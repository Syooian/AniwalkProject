using AniwalkServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AniwalkServer.ViewComponents
{
    public class VC_IndexVisitsPhotos : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly VisitsServices VisitsServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitsServices"></param>
        public VC_IndexVisitsPhotos(VisitsServices VisitsServices)
        {
            this.VisitsServices = VisitsServices;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Result = await VisitsServices.GetRandomVisitsPhotos(10);

            return View(Result);
        }
    }
}
