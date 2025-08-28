using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;

namespace AniwalkServer.ViewComponents
{
    public class VC_SimpleDetail : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public VC_SimpleDetail(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int VisitSN)
        {
            Console.WriteLine($"Invoke VC_SimpleDetail with VisitSN : {VisitSN}");

            var Visit = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

            return View(Visit);
        }
    }
}
