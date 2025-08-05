using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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
        /// <param name="PhotoUpload">上傳的圖片</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Create([Bind("MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,VisitedDate,VisitsPhotos")] Visits Visit, IEnumerable<IFormFile>? PhotoUpload)
        {
            if (ModelState.IsValid)
            {
                Visit.CreatedDate = DateTime.Now;

                //Debug.WriteLine("Visit MemberID : " + Visit.MemberID);

                var UploadMsg = OnVisitPhotoChanged(Visit.VisitsPhotos, PhotoUpload);
                if (UploadMsg != "")
                {
                    ViewData["PhotoError"] = UploadMsg;
                    return View(Visit);
                }

                Context.Add(Visit);
                await Context.SaveChangesAsync();

                return RedirectToAction(nameof(ShowVisitsOnList));
            }

            Shared.ShowModelState(ModelState);

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

            //Debug.WriteLine($"VP Count 1 : " + Visit.VisitsPhotos.Count());
            //foreach (var VP in Visit.VisitsPhotos)
            //{
            //    Debug.WriteLine(VP.ToString());
            //}

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
        /// <param name="PhotoUpload">上傳的圖片</param>
        /// <param name="DeletedPhoto">要刪除的圖片</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SN,MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,CreatedDate,VisitedDate,VisitsPhotos")] Visits Visit,
            IEnumerable<IFormFile>? PhotoUpload,
            List<string>? DeletedPhoto)
        {
            //if (id != tStudent.fStuId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                //Debug.WriteLine($"VP Count 2 : " + Visit.VisitsPhotos.Count());
                //foreach (var VP in Visit.VisitsPhotos)
                //{
                //    Debug.WriteLine(VP.ToString());
                //}

                var UploadMsg = OnVisitPhotoChanged(Visit.VisitsPhotos, PhotoUpload);
                if (UploadMsg != "")
                {
                    ViewData["PhotoError"] = UploadMsg;
                    return View(Visit);
                }

                //刪除照片
                if (DeletedPhoto != null)
                    DeletePhoto(DeletedPhoto);

                //Debug.WriteLine($"VP Count 3 : " + Visit.VisitsPhotos.Count());
                //foreach (var VP in Visit.VisitsPhotos)
                //{
                //    Debug.WriteLine(VP.ToString());
                //}

                Context.Update(Visit);
                await Context.SaveChangesAsync();

                return RedirectToAction(nameof(ShowVisitsOnList));
            }

            Shared.ShowModelState(ModelState);

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

            DeletePhoto(VisitsPhotos.Select(P => P.PhotoID + P.PhotoType).ToList());
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
            //ViewData["MemberID"] = new SelectList(Context.Members, "MemberID", "MemberID",
            //    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            ViewData["MemberID"] = GetMemberID;

            //Console.WriteLine(ViewData["CountryCode"]);
            //Console.WriteLine(ViewData["AnimeID"]);
            //Console.WriteLine("MemberID 1 : " + ViewData["MemberID"]);
            //Console.WriteLine("MemberID 2 : " + User.FindFirstValue(ClaimTypes.NameIdentifier));
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

        /// <summary>
        /// 到訪紀錄照片有變動
        /// </summary>
        /// <param name="VisitPhotoData">圖片資料</param>
        /// <param name="PhotoUpload">上傳的圖片</param>
        /// <returns></returns>
        string OnVisitPhotoChanged(List<VisitsPhotos> VisitPhotoData, IEnumerable<IFormFile>? PhotoUpload)
        {
            #region 有圖片上傳
            if (PhotoUpload != null)
            {
                Debug.WriteLine("新增圖片數量 : " + PhotoUpload.Count());

                if (VisitPhotoData != null)
                {
                    Debug.WriteLine($"Visit.VisitsPhotos Count : {VisitPhotoData.Count()}");

                    foreach (var PhotoData in VisitPhotoData)
                    {
                        Debug.WriteLine(PhotoData.ToString());
                    }
                }
                else
                {
                    //Console.WriteLine("Visit.VisitsPhotos is null");
                    Debug.WriteLine("Visit.VisitsPhotos is null");
                }

                try
                {
                    var PhotoUploadList = PhotoUpload.ToList();

                    for (int a = 0; a < PhotoUploadList.Count(); a++)
                    {
                        if (PhotoUploadList[a] != null && PhotoUploadList[a].Length != 0)
                        {
                            switch (PhotoUploadList[a].ContentType)
                            {
                                case "image/gif":
                                case "image/bmp":
                                case "image/jpg":
                                case "image/jpeg":
                                case "image/png":
                                case "image/jfif":
                                    break;
                                default:
                                    return "不支援的圖片類型";
                            }

                            //新檔名
                            //var FileName = Guid.NewGuid().ToString();
                            var FileName = VisitPhotoData[a].PhotoID;
                            //副檔名
                            //var FileExtension = Path.GetExtension(Photo.FileName).ToLower();
                            var FileExtension = VisitPhotoData[a].PhotoType;
                            Debug.WriteLine($"NewFile : {FileName + FileExtension}");
                            //上傳路徑
                            var UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, GetMemberID);
                            Debug.WriteLine($"UploadPath : {UploadPath}");
                            //檢查上傳路徑
                            if (!Directory.Exists(UploadPath))
                                Directory.CreateDirectory(UploadPath);
                            //上傳
                            using (FileStream FS = new FileStream(Path.Combine(UploadPath, FileName + FileExtension), FileMode.Create))
                            {
                                PhotoUploadList[a].CopyTo(FS);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error uploading photos : {ex.Message}");
                    return "上傳失敗";
                }
            }
            #endregion

            #region 檢查是否有刪除圖片

            #endregion

            return "";
        }

        /// <summary>
        /// 刪除照片
        /// </summary>
        /// <param name="PhotoName">照片檔名<para>含副檔名</para></param>
        void DeletePhoto(List<string> PhotoName)
        {
            for (int a = 0; a < PhotoName.Count; a++)
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, GetMemberID, PhotoName[a]);

                if (System.IO.File.Exists(FilePath))
                {
                    System.IO.File.Delete(FilePath); //刪除照片檔案
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        string GetMemberID
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }
    }
}
