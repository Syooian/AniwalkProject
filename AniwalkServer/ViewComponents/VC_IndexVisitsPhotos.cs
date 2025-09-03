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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Amount">照片數量</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(int Amount = 10)
        {
            var Result = await VisitsServices.GetRandomVisitsPhotos(Amount);

            return View(Result);
        }
    }
}
