using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;
using AniwalkServer.Services;
using System.Diagnostics;

namespace AniwalkServer.ViewComponents
{
    public class VC_SimpleDetail : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly VisitsServices VisitsServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitsServices"></param>
        public VC_SimpleDetail(VisitsServices VisitsServices)
        {
            this.VisitsServices = VisitsServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int VisitSN)
        {
            Debug.WriteLine($"VC_SimpleDetail VisitSN : {VisitSN}");

            var Visit = await VisitsServices.GetVisit(VisitSN, false, false, false, SortPhotos: true);

            return View(Visit);
        }
    }
}
