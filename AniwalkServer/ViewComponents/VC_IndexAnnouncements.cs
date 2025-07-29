using AniwalkServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.ViewComponents
{
    public class VC_IndexAnnouncements : ViewComponent
    {
        private readonly AniwalkDBContext Context;

        public VC_IndexAnnouncements(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Skip">跳過開頭幾筆紀錄</param>
        /// <param name="Take">取幾筆紀錄</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int Skip, int Take)
        {
            var Result = await Context.Announcements.OrderByDescending(A => A.CreatedDate)
                .Skip(Skip)//跳過開頭幾筆紀錄
                .Take(Take)//取幾筆紀錄
                .ToListAsync();

            return View(Result);
        }
    }
}
