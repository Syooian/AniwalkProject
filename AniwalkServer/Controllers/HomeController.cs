using System.Diagnostics;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace AniwalkServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowMap()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
