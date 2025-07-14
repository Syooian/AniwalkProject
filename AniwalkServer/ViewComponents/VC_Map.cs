using AniwalkServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AniwalkServer.ViewComponents
{
    public class VC_Map : ViewComponent
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration Configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Configuration"></param>
        public VC_Map(AniwalkDBContext Context, IConfiguration Configuration)
        {
            this.Context = Context;
            this.Configuration = Configuration;
        }

        /// <summary>
        /// 預設地圖中心
        /// </summary>
        const double DefaultMapCenterLatitude = 22.593469753520136;
        const double DefaultMapCenterLongitude = 120.3088710110155;


        public async Task<IViewComponentResult> InvokeAsync()
        {
            Console.WriteLine($"Invoke VC_Map 1");

            SetMapData(DefaultMapCenterLatitude, DefaultMapCenterLongitude);

            var VM = new VM_Visits
            {
                //Where : 帶入條件

                Countries = await Context.Countries.ToListAsync(),
                Animes = await Context.Animes.ToListAsync(),
                Members = await Context.Members.ToListAsync(),
                Visits = await Context.Visits.OrderByDescending(V => V.CreatedDate).ToListAsync()
                //Students = string.IsNullOrEmpty(id) ? Context.tStudent.ToList() : Context.tStudent.Where(S => S.DeptID == id).ToList()
            };

            Console.WriteLine("Return");

            return View(VM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MapCenterLatitude"></param>
        /// <param name="MapCenterLongitude"></param>
        /// <returns></returns>
        //public async Task<IViewComponentResult> InvokeAsync(double MapCenterLatitude , double MapCenterLongitude )
        //{
        //    Console.WriteLine($"Invoke VC_Map 2 with MapCenterLatitude: {MapCenterLatitude}, MapCenterLongitude: {MapCenterLongitude}");

        //    SetMapData(MapCenterLatitude, MapCenterLongitude);

        //    var VM = new VM_Visits
        //    {
        //        //Where : 帶入條件

        //        Countries = await Context.Countries.ToListAsync(),
        //        Animes = await Context.Animes.ToListAsync(),
        //        Members = await Context.Members.ToListAsync(),
        //        Visits = await Context.Visits.OrderByDescending(V => V.CreatedDate).ToListAsync()
        //        //Students = string.IsNullOrEmpty(id) ? Context.tStudent.ToList() : Context.tStudent.Where(S => S.DeptID == id).ToList()
        //    };

        //    return View(VM);
        //}

        /// <summary>
        /// 設定地圖資料
        /// </summary>
        /// <param name="MapCenterLatitude"></param>
        /// <param name="MapCenterLongitude"></param>
        void SetMapData(double? MapCenterLatitude = null, double? MapCenterLongitude = null)
        {
            //設定Google Maps API金鑰
            ViewBag.GoogleMapsApiKey = Configuration["GoogleMapAPIKey"];

            //Console.WriteLine($"Key : {Configuration["GoogleMapAPIKey"]}");

            ViewBag.MapCenterLatitude = MapCenterLatitude == null ? DefaultMapCenterLatitude : MapCenterLatitude;
            ViewBag.MapCenterLongitude = MapCenterLongitude == null ? DefaultMapCenterLongitude : MapCenterLongitude;

            Console.WriteLine($"MapCenterLatitude: {ViewBag.MapCenterLatitude}, MapCenterLongitude: {ViewBag.MapCenterLongitude}");
        }
    }
}
