using AniwalkServer.Data;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AniwalkServer.ViewComponents
{
    public class VC_VisitsFilter : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View("~/Views/Visits/Shared/_VisitsFilter.cshtml"));
        }
    }
}
