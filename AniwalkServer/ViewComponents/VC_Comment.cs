using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;
using AniwalkServer.Services;

namespace AniwalkServer.ViewComponents
{
    public class VC_Comment : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        readonly CommentsServices CommentsServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="CommentsServices"></param>
        public VC_Comment(AniwalkDBContext Context, CommentsServices CommentsServices)
        {
            this.Context = Context;
            this.CommentsServices = CommentsServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <param name="IsChange">編輯 or 刪除</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int VisitSN, int Page = 1, int PageSize = (int)DefaultPageSize.PageSize_20, bool IsChange = false)
        {
            Console.WriteLine($"Invoke VC_Comment with VisitSN: {VisitSN}, IsChange: {IsChange}");

            var Result = await CommentsServices.GetComments(VisitSN, User.IsInRole(Shared.Role_Admin) ? true : false);

            if (IsChange)
                return View("Change", Result);

            return View(Result);
        }
    }
}
