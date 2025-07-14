using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IActionResult> ShowVisitsOnList()
        {
            SetGoogleMapsApiKey();

            var VM = new VM_Visits
            {
                //Where : 帶入條件

                Countries = await Context.Countries.ToListAsync(),
                Animes = await Context.Animes.ToListAsync(),
                Members = await Context.Members.ToListAsync(),
                Visits = await Context.Visits.OrderByDescending(V => V.CreatedDate).ToListAsync()
                //Students = string.IsNullOrEmpty(id) ? Context.tStudent.ToList() : Context.tStudent.Where(S => S.DeptID == id).ToList()
            };

            //if (!string.IsNullOrEmpty(id))
            //    ViewData["DeptName"] = Context.Department.Find(id).DeptName;
            //ViewData["DeptID"] = id;

            return View(VM);
        }

        /// <summary>
        /// 創建新的到訪記錄
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            SetGoogleMapsApiKey();

            SetViewData();

            return View();
        }
        /// <summary>
        /// 送出新建到訪記錄表單
        /// </summary>
        /// <param name="Visit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID")] Visits Visit)
        {
            if (ModelState.IsValid)
            {
                Visit.CreatedDate = DateTime.Now;

                Context.Add(Visit);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowVisitsOnList));
            }

            SetViewData();

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int VisitSN)
        {
            //Console.WriteLine($"Edit VisitSN : {VisitSN}");

            //if (VisitSN == null)
            //{
            //    Console.WriteLine("VisitSN is null");

            //    return NotFound();
            //}

            var Visit = await Context.Visits.FindAsync(VisitSN);
            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");

                return NotFound();
            }

            SetViewData();

            return View(Visit);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Visit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SN,MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,CreatedDate")] Visits Visit)
        {
            //if (id != tStudent.fStuId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(Visit);

                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsStudentExists(Visit.SN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ShowVisitsOnList));
            }

            SetViewData();

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int VisitSN)
        {
            var Visit = await Context.Visits.FindAsync(VisitSN);
            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");

                return NotFound();
            }

            SetViewData();

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetViewData()
        {
            ViewData["CountryCode"] = new SelectList(Context.Countries, "CountryCode", "CountryName");
            ViewData["AnimeID"] = new SelectList(Context.Animes, "AnimeID", "Title");
            ViewData["MemberID"] = new SelectList(Context.Members, "MemberID", "Name");//臨時
        }

        /// <summary>
        /// 設定Google Maps API金鑰
        /// </summary>
        public void SetGoogleMapsApiKey()
        {
            ViewBag.GoogleMapsApiKey = _configuration["GoogleMapAPIKey"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        private bool IsStudentExists(int VisitSN)
        {
            return Context.Visits.Any(V => V.SN == VisitSN);
        }
    }
}
