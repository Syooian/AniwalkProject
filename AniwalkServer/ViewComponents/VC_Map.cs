using AniwalkServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using AniwalkServer.Data;
using AniwalkServer.Services;
using System.Diagnostics;
using AniwalkServer.QueryParameters;
using AniwalkServer.DTOs;

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
        readonly VisitsServices VisitsServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Configuration"></param>
        public VC_Map(AniwalkDBContext Context, IConfiguration Configuration, VisitsServices VisitsServices)
        {
            this.Context = Context;
            this.Configuration = Configuration;
            this.VisitsServices = VisitsServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MapDataParam"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(MapDataParam MapDataParam, List<VisitsDTO> Model)
        {
            Debug.WriteLine($"Invoke VC_Map 1");

            SetMapData(MapDataParam);

            //var VM = new VM_Visits
            //{
            //    //Where : 帶入條件

            //    Countries = await Context.Countries.ToListAsync(),
            //    Animes = await Context.Animes.ToListAsync(),
            //    Members = await Context.Members.ToListAsync(),
            //    Visits = await Context.Visits.OrderByDescending(V => V.CreatedDate).ToListAsync()
            //    //Students = string.IsNullOrEmpty(id) ? Context.tStudent.ToList() : Context.tStudent.Where(S => S.DeptID == id).ToList()
            //};

            //Console.WriteLine("Return");

            return View(Model);
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
        /// <param name="MapDataParam"></param>
        void SetMapData(MapDataParam MapDataParam)
        {
            //設定Google Maps API金鑰
            SetGoogleMapsApiKey();

            //Console.WriteLine($"Key : {Configuration["GoogleMapAPIKey"]}");

            //ViewBag.MapCenterLatitude = MapCenterLatitude;
            //ViewBag.MapCenterLongitude = MapCenterLongitude;
            ViewBag.MapData = MapDataParam;

            //Debug.WriteLine($"MapCenterLatitude: {ViewBag.MapCenterLatitude}, MapCenterLongitude: {ViewBag.MapCenterLongitude}");
        }

        /// <summary>
        /// 設定Google Maps API金鑰
        /// </summary>
        void SetGoogleMapsApiKey()
        {
            ViewBag.GoogleMapsApiKey = Configuration["GoogleMapAPIKey"];
        }
    }
}
