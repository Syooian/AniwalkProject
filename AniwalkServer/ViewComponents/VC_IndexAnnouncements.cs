using AniwalkServer.Data;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AniwalkServer.ViewComponents
{
    public class VC_IndexAnnouncements : ViewComponent
    {
        readonly AnnouncementsServices AnnouncementsServices;

        public VC_IndexAnnouncements(AnnouncementsServices AnnouncementsServices)
        {
            this.AnnouncementsServices = AnnouncementsServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int Page = 1)
        {
            var Result = await AnnouncementsServices.GetAnnouncements(Page, (int)DefaultPageSize.PageSize_5);

            return View(Result);
        }
    }
}
