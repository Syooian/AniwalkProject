using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;
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
        [AllowAnonymous]
        public async Task<IActionResult> ShowVisitsOnMap()
        {
            SetGoogleMapsApiKey();

            #region 在資料庫執行排序
            var Result = await Context.Visits.Include(V => V.Member).ToListAsync();
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

            return View(Result.ToArray());
        }

        /// <summary>
        /// 從清單瀏覽到訪紀錄
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]//允許所有人檢視
        public async Task<IActionResult> ShowVisitsOnList()
        {
            var Result = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .OrderByDescending(V => V.CreatedDate)
                .ToListAsync();

            return View(Result);
        }

        /// <summary>
        /// 創建新的到訪記錄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Shared.Role_Member)]
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
        /// <param name="VisitsPhotos">到訪紀錄照片</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Create([Bind("MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,VisitedDate,VisitsPhotos")] Visits Visit, IEnumerable<IFormFile>? VisitsPhotos)
        {
            if (ModelState.IsValid)
            {
                Visit.CreatedDate = DateTime.Now;

                Context.Add(Visit);
                await Context.SaveChangesAsync();

                if (VisitsPhotos != null)
                {
                    if (Visit.VisitsPhotos != null)
                    {
                        Debug.WriteLine($"Visit.VisitsPhotos Count : {Visit.VisitsPhotos.Count()}");

                        foreach (var Des in Visit.VisitsPhotos)
                        {
                            //Console.WriteLine($"Visit.VisitsPhotos Description: {Des.Description}");
                            Debug.WriteLine($"Visit.VisitsPhotos Description: {Des.Description}");
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Visit.VisitsPhotos is null");
                        Debug.WriteLine("Visit.VisitsPhotos is null");
                    }

                    foreach (var Photo in VisitsPhotos)
                    {
                        if (Photo != null && Photo.Length != 0)
                        {
                            switch (Photo.ContentType)
                            {
                                case "image/gif":
                                case "image/bmp":
                                case "image/jpg":
                                case "image/jpeg":
                                case "image/png":
                                case "image/jfif":
                                    break;
                                default:
                                    ViewData["PhotoError"] = "不支援的圖片類型";
                                    return View(Visit);
                            }

                            //新檔名
                            var FileName = Guid.NewGuid().ToString();
                            //副檔名
                            var FileExtension = Path.GetExtension(Photo.FileName).ToLower();
                            //上傳路徑
                            var UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, Visit.MemberID);
                            //檢查上傳路徑
                            if (!Directory.Exists(UploadPath))
                                Directory.CreateDirectory(UploadPath);
                            //上傳
                            using (FileStream FS = new FileStream(Path.Combine(UploadPath, FileName + FileExtension), FileMode.Create))
                            {
                                Photo.CopyTo(FS);
                            }

                            //儲存新檔名到資料庫
                            Context.VisitsPhotos.Add(new VisitsPhotos
                            {
                                MemberID = Visit.MemberID,
                                SN = Visit.SN,
                                PhotoID = FileName,
                                PhotoType = FileExtension,
                                //Description = Visit.VisitsPhotos[async].Description
                            });
                        }
                    }

                    await Context.SaveChangesAsync();
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
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Edit(int VisitSN)
        {
            //Console.WriteLine($"Edit VisitSN : {VisitSN}");

            //if (VisitSN == null)
            //{
            //    Console.WriteLine("VisitSN is null");

            //    return NotFound();
            //}

            var Visit = await Context.Visits
                .Include(V => V.VisitsPhotos)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

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
        public async Task<IActionResult> Edit([Bind("SN,MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,CreatedDate,VisitedDate")] Visits Visit)
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
            var Visit = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");

                return NotFound();
            }

            SetGoogleMapsApiKey();

            //SetViewData();

            return View(Visit);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public ViewComponentResult SimpleDetails(int VisitSN) => ViewComponent("VC_SimpleDetail", new { VisitSN });
        //public ViewComponentResult SimpleDetails(int VisitSN)
        //{
        //    return ViewComponent("VC_SimpleDetail", new { VisitSN }); ;
        //}

        /// <summary>
        /// 刪除到訪紀錄
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int VisitSN)
        {
            //Console.WriteLine($"Delete VisitSN : {VisitSN}");

            var Visit = await Context.Visits.FindAsync(VisitSN);
            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");
                return NotFound();
            }

            #region 刪除到訪紀錄照片
            var VisitsPhotos = await Context.VisitsPhotos.Where(VP => Visit.SN == VP.SN).ToListAsync();
            foreach (var Photo in VisitsPhotos)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VisitsPhotos", Photo.PhotoID + Photo.PhotoType);

                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath); //刪除圖片檔案
                }
            }
            #endregion

            Context.Visits.Remove(Visit);
            await Context.SaveChangesAsync();

            //SetGoogleMapsApiKey();
            return RedirectToAction(nameof(ShowVisitsOnList));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetViewData()
        {
            ViewData["CountryCode"] = new SelectList(Context.Countries, "CountryCode", "CountryName");
            ViewData["AnimeID"] = new SelectList(Context.Animes, "AnimeID", "Title");
            ViewData["MemberID"] = new SelectList(Context.Members, "MemberID", "MemberID",
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            //Console.WriteLine(ViewData["CountryCode"]);
            //Console.WriteLine(ViewData["AnimeID"]);
            //Console.WriteLine(ViewData["MemberID"]);
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
