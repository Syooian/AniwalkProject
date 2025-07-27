using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;

namespace AniwalkServer.ViewComponents
{
    public class VC_Reply : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public VC_Reply(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="ParentReplyID"></param>
        /// <param name="IsChange">編輯 or 刪除</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int VisitSN, string ParentReplyID, bool IsChange = false)
        {
            Console.WriteLine($"Invoke VC_Reply with VisitSN : {VisitSN}, ParentReplyID : {ParentReplyID}, IsChange : {IsChange}");

            var Result = await Context.Comments
                .Where(V => V.SN == VisitSN)
                .Include(V => V.Replies).ThenInclude(R => R.Member)
                .Include(V => V.Replies).ThenInclude(R => R.ParentReply)
                .OrderByDescending(C => C.CommentDate).ToListAsync();

            if (IsChange)
                return View("Change", Result);

            return View(Result);
        }
    }
}
