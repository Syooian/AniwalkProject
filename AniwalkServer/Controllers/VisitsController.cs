using Microsoft.AspNetCore.Mvc;

namespace AniwalkServer.Controllers
{
    public class VisitsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public VisitsController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 從地圖瀏覽到訪紀錄
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowVisitsOnMap()
        {
            ViewBag.GoogleMapsApiKey = _configuration["GoogleMapAPIKey"];


            var Markers = new List<object>
            {
                new{Lat=22.589893781702656,Lng= 120.31014242236083,Title="Marker 1" },
                new{Lat=22.59165699959131,Lng= 120.31737356655549,Title="Marker 2" }
            };

            ViewBag.Markers = Markers;

            return View();
        }

        /// <summary>
        /// 從清單瀏覽到訪紀錄
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowVisitsOnList()
        {
            return View();
        }
    }
}
