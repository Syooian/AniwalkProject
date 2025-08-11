using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using AniwalkServer.Services;
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
        #region Services
        /// <summary>
        /// 
        /// </summary>
        readonly VisitsServices VisitsServices;
        /// <summary>
        /// 
        /// </summary>
        readonly PhotoServices PhotoServices;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="Context"></param>
        /// <param name="VisitsServices"></param>
        /// <param name="PhotoServices"></param>
        public VisitsController(ILogger<HomeController> logger, IConfiguration configuration, AniwalkDBContext Context, VisitsServices VisitsServices, PhotoServices PhotoServices)
        {
            _logger = logger;
            _configuration = configuration;
            this.Context = Context;
            #region Services
            this.VisitsServices = VisitsServices;
            this.PhotoServices = PhotoServices;
            #endregion
        }

        /// <summary>
        /// 從地圖瀏覽到訪紀錄
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="AnimeTitle"></param>
        /// <param name="MemberName"></param>
        /// <param name="VisitedDate"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> ShowVisitsOnMap(string? CountryName, string? AnimeTitle, string? MemberName, string? VisitedDate)
        {
            SetGoogleMapsApiKey();

            #region 在資料庫執行排序
            //var Result = await Context.Visits.Include(V => V.Member).ToListAsync();
            var Result = await VisitsServices.GetVisits(CountryName, AnimeTitle, MemberName, VisitedDate);
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

            ViewData["AJAXAction"] = nameof(ShowVisitsOnMap);

            return View(Result.ToArray());
        }

        /// <summary>
        /// 從清單瀏覽到訪紀錄
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="AnimeTitle"></param>
        /// <param name="MemberName"></param>
        /// <param name="VisitedDate"></param>
        /// <returns></returns>
        [AllowAnonymous]//允許所有人檢視
        public async Task<IActionResult> ShowVisitsOnList(VisitsParam? VisitsParam)
        {
            //if (VisitsParam != null)
            //    Debug.WriteLine(VisitsParam.ToString());

            var Result = await VisitsServices.GetVisits(VisitsParam);

            ViewData["AJAXAction"] = nameof(ShowVisitsOnList);

            // 判斷是否為 AJAX 請求
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                Debug.WriteLine("Return AJAX");

                // 回傳部分視圖（只渲染清單）
                return PartialView("_VisitsList", Result); // _VisitsList.cshtml 需只渲染清單
            }
            else// 一般頁面載入
            {
                Debug.WriteLine("Return View");

                return View(Result);
            }
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
        /// <param name="VisitPhotos">圖片資料</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Create([Bind("MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,VisitedDate,VisitsPhotos")] Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (ModelState.IsValid)
            {
                Visit.CreatedDate = DateTime.Now;

                //Debug.WriteLine("Visit MemberID : " + Visit.MemberID);

                var UploadMsg = await UploadPhoto(Visit, false, VisitPhotos);
                if (UploadMsg != "")
                {
                    ViewData["PhotoError"] = UploadMsg;
                    return View(Visit);
                }

                if (VisitPhotos != null)
                {
                    Visit.VisitsPhotos = new List<VisitsPhotos>();
                    for (int a = 0; a < VisitPhotos.Count(); a++)
                    {
                        Visit.VisitsPhotos.Add(new VisitsPhotos
                        {
                            PhotoID = VisitPhotos[a].PhotoID,
                            PhotoType = VisitPhotos[a].PhotoType,
                            Description = VisitPhotos[a].Description,
                            MemberID = GetMemberID,
                            SN = Visit.SN
                        });
                    }
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

            //var Visit = await Context.Visits
            //    .Include(V => V.VisitsPhotos)
            //    .FirstOrDefaultAsync(V => V.SN == VisitSN);
            var Visit = await VisitsServices.GetVisit(VisitSN, true);

            //Debug.WriteLine($"VP Count 1 : " + Visit.VisitsPhotos.Count());
            //for (int a = 0; a < Visit.VisitsPhotos.Count(); a++)
            //{
            //    Debug.WriteLine(Visit.VisitsPhotos[a].ToString());
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
        /// <param name="VisitPhotos">圖片資料</param>
        /// <param name="DeletePhoto">要刪除的圖片</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SN,MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,CreatedDate,VisitedDate,VisitsPhotos")] Visits Visit,
            List<VisitsPhotosDTO>? VisitPhotos,
            List<string>? DeletePhoto)
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

                #region 測試1
                //Visit.VisitsPhotos = new List<VisitsPhotos>();
                //Debug.WriteLine($"VP Count 2-1 : " + Visit.VisitsPhotos.Count());
                //Debug.WriteLine($"VP Count 2-2 : " + VisitPhotos.Count());
                //for (int a = 0; a < VisitPhotos.Count(); a++)
                //{
                //    Debug.WriteLine(VisitPhotos[a].ToString());
                //    Visit.VisitsPhotos.Add(new VisitsPhotos
                //    {
                //        PhotoID = VisitPhotos[a].PhotoID,
                //        PhotoType = VisitPhotos[a].PhotoType,
                //        Description = VisitPhotos[a].Description,
                //        MemberID = GetMemberID,
                //        SN = Visit.SN
                //    });
                //}
                #endregion

                var UploadPhotoMsg = await UploadPhoto(Visit, true, VisitPhotos);
                if (UploadPhotoMsg != "")
                {
                    ViewData["PhotoError"] = UploadPhotoMsg;
                    return View(Visit);
                }

                #region 測試2
                //var VP = VisitPhotos.FindAll(VP => VP.UploadFile == null);
                //Visit.VisitsPhotos = new List<VisitsPhotos>();
                //Debug.WriteLine($"VP Count 2-1 : " + Visit.VisitsPhotos.Count());
                //Debug.WriteLine($"VP Count 2-2 : " + VP.Count());
                //for (int a = 0; a < VP.Count(); a++)
                //{
                //    Debug.WriteLine(VP[a].ToString());
                //    Visit.VisitsPhotos.Add(new VisitsPhotos
                //    {
                //        PhotoID = VP[a].PhotoID,
                //        PhotoType = VP[a].PhotoType,
                //        Description = VP[a].Description,
                //        MemberID = GetMemberID,
                //        SN = Visit.SN
                //    });
                //}
                #endregion

                var UpdatePhotoDataMsg = await UpdatePhotoData(Visit, VisitPhotos);
                if (UpdatePhotoDataMsg != "")
                {
                    ViewData["PhotoError"] = UpdatePhotoDataMsg;
                    return View(Visit);
                }

                //(寫法不佳)
                //Visit.VisitsPhotos = new List<VisitsPhotos>();
                //foreach (var VP in VisitPhotos)
                //{
                //    if (VP.UploadFile == null)
                //    {
                //        Visit.VisitsPhotos.Add(new VisitsPhotos()
                //        {
                //            PhotoID = VP.PhotoID,
                //            PhotoType = VP.PhotoType,
                //            Description = VP.Description,
                //            MemberID = Visit.MemberID,
                //            SN = Visit.SN
                //        });
                //    }
                //}

                //刪除照片
                await PhotoServices.DeletePhoto(Visit, VisitPhotos);

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
            var Visit = await VisitsServices.GetVisit(VisitSN, true);

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

            //var Visit = await Context.Visits
            //    .Include(V => V.VisitsPhotos)
            //    .FirstOrDefaultAsync(V => V.SN == VisitSN);
            var Visit = await VisitsServices.GetVisit(VisitSN);

            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");
                return NotFound();
            }

            //刪除到訪紀錄照片
            PhotoServices.DeletePhoto(Visit);

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
        /// <param name="Visit"></param>
        /// <param name="VisitPhotos"></param>
        /// <returns></returns>
        async Task<string> UpdatePhotoData(Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (VisitPhotos == null || VisitPhotos.Count == 0)
            {
                Debug.WriteLine("沒有圖片資料更新");
                return await Task.FromResult("");
            }

            //Debug.WriteLine($"UpdatePhotoData VisitPhotos Count : {VisitPhotos.Count()}");
            var UpdatePhotoData = VisitPhotos.FindAll(VP => VP.UploadFile == null);
            foreach (var PhotoData in UpdatePhotoData)
            {
                //Debug.WriteLine(VP.ToString());
                var Original = await Context.VisitsPhotos.FirstOrDefaultAsync(V => V.PhotoID == PhotoData.PhotoID);
                if (Original != null && Original.Description != PhotoData.Description)//有相同資料，用修改的 (也只有圖片說明會更改)
                {
                    Debug.WriteLine($"修改圖片資料");
                    Original.Description = PhotoData.Description;
                    Context.Update(Original);
                }
            }

            return await Task.FromResult("");
        }

        /// <summary>
        /// 到訪紀錄照片有變動
        /// </summary>
        /// <param name="Visit">到訪記錄</param>
        /// <param name="VisitPhotos">圖片資料</param>
        /// <param name="DeletePhoto">上傳的圖片</param>
        /// <returns></returns>
        /// 修正 Task<string> 回傳型別，將所有 return "字串" 改為 return Task.FromResult("字串")
        //async Task<string> OnVisitPhotoChanged(Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        //{
        //    if (VisitPhotos == null || VisitPhotos.Count == 0)
        //    {
        //        Debug.WriteLine("沒有上傳變動");
        //        return await Task.FromResult("");
        //    }

        //    #region 檢查是否有上傳圖片
        //    if (VisitPhotos != null)
        //    {
        //        Debug.WriteLine("圖片資料數量 : " + VisitPhotos.Count());

        //        try
        //        {
        //            for (int a = 0; a < VisitPhotosList.Count(); a++)
        //            {
        //                //檢查是否有圖片要上傳
        //                Debug.WriteLine($"{VisitPhotosList[a].PhotoID} 圖片 : {(VisitPhotosList[a].UploadFile == null ? null : VisitPhotosList[a].UploadFile.FileName)}");
        //                if (VisitPhotosList[a].UploadFile != null && VisitPhotosList[a].UploadFile.Length != 0)
        //                {
        //                    //檢查檔案類型
        //                    switch (VisitPhotosList[a].UploadFile.ContentType)
        //                    {
        //                        case "image/gif":
        //                        case "image/bmp":
        //                        case "image/jpg":
        //                        case "image/jpeg":
        //                        case "image/png":
        //                        case "image/jfif":
        //                            break;
        //                        default:
        //                            return await Task.FromResult("有不支援的圖片類型");
        //                    }

        //                    //上傳路徑
        //                    var UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, GetMemberID);
        //                    Debug.WriteLine($"UploadPath : {UploadPath}");
        //                    //檢查上傳路徑
        //                    if (!Directory.Exists(UploadPath))
        //                        Directory.CreateDirectory(UploadPath);
        //                    //上傳
        //                    using (FileStream FS = new FileStream(Path.Combine(UploadPath, VisitPhotosList[a].PhotoID + VisitPhotosList[a].PhotoType), FileMode.Create))
        //                    {
        //                        VisitPhotosList[a].UploadFile.CopyTo(FS);
        //                    }

        //                    //Context.VisitsPhotos.Add(new VisitsPhotos
        //                    //{
        //                    //    PhotoID = VisitPhotosList[a].PhotoID,
        //                    //    PhotoType = VisitPhotosList[a].PhotoType,
        //                    //    Description = VisitPhotosList[a].Description,
        //                    //    MemberID = GetMemberID,
        //                    //    SN = Visit.SN
        //                    //});

        //                    //IsModified = true;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Error uploading photos : {ex.Message}");
        //            return await Task.FromResult("上傳失敗1");
        //        }
        //    }
        //    else
        //    {
        //        Debug.WriteLine("沒有圖片資料");
        //    }
        //    #endregion

        //    #region 檢查是否有刪除圖片

        //    #endregion


        //    //等待一次增加新圖片資料到資料庫
        //    //if (IsModified)
        //    //    await Context.SaveChangesAsync();

        //    #region 更新資料庫
        //    //if (Visit.VisitsPhotos == null)
        //    //{
        //    //    Debug.WriteLine($"New Visit.VisitsPhotos");
        //    //    Visit.VisitsPhotos = new List<VisitsPhotos>();
        //    //}
        //    //else
        //    //{
        //    //    Debug.WriteLine($"Visit.VisitsPhotos Count : {Visit.VisitsPhotos.Count()}");
        //    //}

        //    //foreach (var VP in VisitPhotos)
        //    //{
        //    //    //找出有無和原圖片資料相同
        //    //    var V = Visit.VisitsPhotos.FindIndex(R => R.PhotoID == VP.PhotoID);

        //    //    if (V == -1)//無相同資料，直接插入新的
        //    //    {
        //    //        Debug.WriteLine($"新增圖片資料");

        //    //        Visit.VisitsPhotos.Add(new VisitsPhotos
        //    //        {
        //    //            PhotoID = VP.PhotoID,
        //    //            PhotoType = VP.PhotoType,
        //    //            Description = VP.Description,
        //    //            MemberID = GetMemberID,
        //    //            SN = Visit.SN
        //    //        });
        //    //    }
        //    //    else//有相同資料，用修改的 (也只有圖片說明會更改)
        //    //    {
        //    //        Debug.WriteLine($"修改圖片資料");

        //    //        Visit.VisitsPhotos[V].Description = VP.Description;
        //    //    }
        //    //}

        //    //追蹤問題??? 樂觀並發控制???

        //    //-----------------------------------------------------------------------------------------------------

        //    foreach (var VP in VisitPhotosList)
        //    {
        //        //找出有無和原圖片資料相同
        //        var Original = await Context.VisitsPhotos.FirstOrDefaultAsync(V => V.PhotoID == VP.PhotoID);
        //        if (Original == null)//無相同資料，直接插入新的
        //        {
        //            Debug.WriteLine($"新增圖片資料");
        //            Context.VisitsPhotos.Add(new VisitsPhotos()
        //            {
        //                PhotoID = VP.PhotoID,
        //                PhotoType = VP.PhotoType,
        //                Description = VP.Description,
        //                MemberID = Visit.MemberID,
        //                SN = Visit.SN
        //            });
        //        }
        //        else if (Original.Description != VP.Description)//有相同資料，用修改的 (也只有圖片說明會更改)
        //        {
        //            Debug.WriteLine($"修改圖片資料");
        //            Original.Description = VP.Description;
        //            Context.Update(Original);
        //        }
        //    }

        //    await Context.SaveChangesAsync();
        //    #endregion

        //    return await Task.FromResult("");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Visit"></param>
        /// <param name="VisitPhotos"></param>
        /// <returns></returns>
        async Task<string> UploadPhoto(Visits Visit, bool SetDBFirst, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (VisitPhotos == null || VisitPhotos.Count == 0)
            {
                Debug.WriteLine("沒有上傳圖片");
                return await Task.FromResult("");
            }

            var UploadPhotos = VisitPhotos.FindAll(VP => VP.UploadFile != null && VP.UploadFile.Length != 0);
            foreach (var Photo in UploadPhotos)
            {
                //檢查檔案類型
                switch (Photo.UploadFile.ContentType)
                {
                    case "image/gif":
                    case "image/bmp":
                    case "image/jpg":
                    case "image/jpeg":
                    case "image/png":
                    case "image/jfif":
                        break;
                    default:
                        return await Task.FromResult("有不支援的圖片類型");
                }

                try
                {
                    //上傳路徑
                    var UploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath, GetMemberID);
                    Debug.WriteLine($"UploadPath : {UploadPath}");
                    //檢查上傳路徑
                    if (!Directory.Exists(UploadPath))
                        Directory.CreateDirectory(UploadPath);
                    //上傳
                    using (FileStream FS = new FileStream(Path.Combine(UploadPath, Photo.PhotoID + Photo.PhotoType), FileMode.Create))
                    {
                        Photo.UploadFile.CopyTo(FS);
                    }

                    if (SetDBFirst)
                    {
                        Context.Add(new VisitsPhotos()
                        {
                            PhotoID = Photo.PhotoID,
                            PhotoType = Photo.PhotoType,
                            Description = Photo.Description,
                            MemberID = GetMemberID,
                            SN = Visit.SN
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error uploading photos : {ex.Message}");
                    return await Task.FromResult("上傳失敗1");
                }
            }

            return await Task.FromResult("");
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
