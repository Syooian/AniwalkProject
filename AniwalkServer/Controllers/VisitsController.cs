using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Controllers
{
    public class VisitsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AniwalkDBContext Context;

        public VisitsController(ILogger<HomeController> logger, IConfiguration configuration, AniwalkDBContext Context)
        {
            _logger = logger;
            _configuration = configuration;
            this.Context = Context;
        }

        /// <summary>
        /// 從地圖瀏覽到訪紀錄
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ShowVisitsOnMap()
        {
            SetGoogleMapsApiKey();

            #region 在資料庫執行排序
            //var Result = await _context.Book.OrderByDescending(R => R.CreatedDate).ToListAsync();
            #endregion
            #region 在本機記憶體執行排序
            //var Result = await _context.Book.ToListAsync();
            //Result.OrderByDescending(R => R.CreatedDate);
            #endregion

            //var Markers = new List<object>
            //{
            //    new{Lat=22.589893781702656,Lng= 120.31014242236083,Title="Marker 1" },
            //    new{Lat=22.59165699959131,Lng= 120.31737356655549,Title="Marker 2" }
            //};

            //var Markers = await Context.Visits.ToListAsync();

            //ViewBag.Markers = Markers;

            return View(await Context.Visits.ToArrayAsync());
        }

        /// <summary>
        /// 從清單瀏覽到訪紀錄
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowVisitsOnList()
        {
            SetGoogleMapsApiKey();

            return View();
        }

        /// <summary>
        /// 設定Google Maps API金鑰
        /// </summary>
        public void SetGoogleMapsApiKey()
        {
            ViewBag.GoogleMapsApiKey = _configuration["GoogleMapAPIKey"];
        }
    }
}
