using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

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
        /// <summary>
        /// 
        /// </summary>
        readonly AnimesServices AnimesServices;
        /// <summary>
        /// 
        /// </summary>
        readonly CountriesServices CountriesServices;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="Context"></param>
        /// <param name="VisitsServices"></param>
        /// <param name="PhotoServices"></param>
        /// <param name="AnimesServices"></param>
        /// <param name="CountriesServices"></param>
        public VisitsController(ILogger<HomeController> logger, IConfiguration configuration, AniwalkDBContext Context, VisitsServices VisitsServices, PhotoServices PhotoServices, AnimesServices AnimesServices, CountriesServices CountriesServices)
        {
            _logger = logger;
            _configuration = configuration;
            this.Context = Context;
            #region Services
            this.VisitsServices = VisitsServices;
            this.PhotoServices = PhotoServices;
            this.AnimesServices = AnimesServices;
            this.CountriesServices = CountriesServices;
            #endregion
        }

        /// <summary>
        /// 從地圖瀏覽到訪紀錄
        /// </summary>
        /// <param name="VisitsParam"></param>
        /// <param name="MapDataParam"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]//補上HttpGet，不然會報錯 (多個EndPoint錯誤)
        public async Task<IActionResult> ShowVisitsOnMap(VisitsParam? VisitsParam, string? MapDataParam)
        {
            SetGoogleMapsApiKey();

            #region 在資料庫執行排序
            //var Result = await Context.Visits.Include(V => V.Member).ToListAsync();
            var Result = await VisitsServices.GetVisits(VisitsParam);
            #endregion
            #region 在本機記憶體執行排序
            //var Result = await _context.Book.ToListAsync();
            //Result.OrderByDescending(R => R.CreatedDate);
            #endregion

            if (Result == null)
                return NotFound();

            if (!string.IsNullOrEmpty(MapDataParam))
            {
                //Debug.WriteLine("ShowVisitsOnMap MapData : " + MapDataParam);
                //ViewData[ViewDataKeys.MapData] = JsonConvert.DeserializeObject<MapDataParam>(MapDataParam);
                ViewData[ViewDataKeys.MapData] = Uri.UnescapeDataString(MapDataParam);//因為在View丟值過來時因為是帶在網頁上，需經過一次URL格式的轉換避免出錯，因此再次塞給View時需轉換回來才會是正確的Json格式
                //Debug.WriteLine("Controller ShowVisitsOnMap 1 MapData : " + MapDataParam);
                //Debug.WriteLine("Controller ShowVisitsOnMap 2 MapData : " + Uri.UnescapeDataString(MapDataParam));
                //Debug.WriteLine("Controller ShowVisitsOnMap 2 MapData : " +JsonConvert.SerializeObject Uri.UnescapeDataString(MapDataParam));
            }
            else
            {
                ViewData[ViewDataKeys.MapData] = JsonConvert.SerializeObject(new MapDataParam());
            }

            //var Markers = new List<object>
            //{
            //    new{Lat=22.589893781702656,Lng= 120.31014242236083,Title="Marker 1" },
            //    new{Lat=22.59165699959131,Lng= 120.31737356655549,Title="Marker 2" }
            //};

            //var Markers = await Context.Visits.ToListAsync();

            //ViewBag.Markers = Markers;

            ViewData[ViewDataKeys.AJAXAction] = nameof(ShowVisitsOnMap);
            await SetViewData();

            // 判斷是否為 AJAX 請求
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                //Debug.WriteLine("Return AJAX");

                // 回傳部分視圖（只渲染清單）
                return Json(Result.Data); // _VisitsList.cshtml 需只渲染清單
            }
            else// 一般頁面載入
            {
                //Debug.WriteLine("Return View");

                return View(Result.Data);
            }
        }

        /// <summary>
        /// 從清單瀏覽到訪紀錄
        /// </summary>
        /// <param name="VisitsParam"></param>
        /// <returns></returns>
        [AllowAnonymous]//允許所有人檢視
        public async Task<IActionResult> ShowVisitsOnList(VisitsParam? VisitsParam, int Page = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        {
            //if (VisitsParam != null)
            //    Debug.WriteLine("ShowVisitsOnList 1 Param : " + VisitsParam.ToString());

            var Result = await VisitsServices.GetVisits(VisitsParam, Page, PageSize, (User.IsInRole(Shared.Role_Admin) ? true : false));

            if (Result == null)
                return NotFound();

            //if (Result.Filter != null)
            //    Debug.WriteLine("ShowVisitsOnList 2 Filter : " + Result.Filter.ToString());

            ViewData[ViewDataKeys.AJAXAction] = nameof(ShowVisitsOnList);
            await SetViewData();

            ViewData[ViewDataKeys.LastPage] = Page;

            // 判斷是否為 AJAX 請求
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                //Debug.WriteLine("Return AJAX");

                // 回傳部分視圖（只渲染清單）
                return PartialView("ShowVisitsOnList.VisitsList", Result); // _VisitsList.cshtml 需只渲染清單
            }
            else// 一般頁面載入
            {
                //Debug.WriteLine("Return View");

                return View(Result);
            }
        }

        /// <summary>
        /// 創建新的到訪記錄
        /// </summary>
        /// <param name="LastPage"></param>
        /// <param name="LastAction"></param>
        /// <param name="MapDataParam"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Create(int? LastPage, string? LastAction, string? MapDataParam)
        {
            SetGoogleMapsApiKey();

            ViewData[ViewDataKeys.AJAXAction] = LastAction;
            ViewData[ViewDataKeys.LastPage] = LastPage;
            ViewData[ViewDataKeys.MapData] = MapDataParam;
            await SetViewData();

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

                var UploadPhotoResult = await VisitsServices.UploadPhoto(Visit, false, VisitPhotos, GetMemberID);
                if (UploadPhotoResult.Type == ResultType.Fail)
                {
                    ViewData["PhotoError"] = UploadPhotoResult.Message;
                    return View(Visit);
                }

                if (VisitPhotos != null)
                {
                    Visit.VisitsPhotos = new List<VisitsPhotos>();
                    for (int a = 0; a < VisitPhotos.Count(); a++)
                    {
                        //Debug.WriteLine($"NewPhoto {a} : {VisitPhotos[a].PhotoID}");

                        Visit.VisitsPhotos.Add(new VisitsPhotos
                        {
                            PhotoID = VisitPhotos[a].PhotoID,
                            PhotoType = VisitPhotos[a].PhotoType,
                            Description = VisitPhotos[a].Description,
                            MemberID = GetMemberID,
                            SN = Visit.SN,
                            SortNumber = a
                        });
                    }
                }

                Context.Add(Visit);
                await Context.SaveChangesAsync();

                return RedirectToAction(nameof(ShowVisitsOnList));
            }

            Shared.ShowModelState(ModelState);

            await SetViewData();

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="LastPage"></param>
        /// <param name="LastAction"></param>
        /// <param name="MapDataParam"></param>
        /// <returns></returns>
        [Authorize(Roles = Shared.Role_Member)]
        [HttpGet]
        public async Task<IActionResult> Edit(int VisitSN, int LastPage, string? LastAction, string? MapDataParam)
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
            var Visit = await VisitsServices.GetVisit(VisitSN, SortPhotos: true);

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

            SetGoogleMapsApiKey();

            await SetViewData();

            ViewData[ViewDataKeys.AJAXAction] = LastAction;
            //Debug.WriteLine("Controller Edit 1 MapData : " + MapDataParam);
            if (!string.IsNullOrEmpty(MapDataParam))
                ViewData[ViewDataKeys.MapData] = Uri.UnescapeDataString(MapDataParam);
            //Debug.WriteLine("Controller Edit 2 MapData : " + Uri.UnescapeDataString(MapDataParam));
            ViewData[ViewDataKeys.LastPage] = LastPage;

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Visit"></param>
        /// <param name="VisitPhotos">圖片資料</param>
        /// <param name="DeletePhoto">要刪除的圖片</param>
        /// <param name="LastPage"></param>
        /// <param name="LastAction"></param>
        /// <param name="MapDataParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SN,MainText,Latitude,Longitude,MemberID,CountryCode,AnimeID,CreatedDate,VisitedDate,VisitsPhotos")] Visits Visit,
            List<VisitsPhotosDTO>? VisitPhotos,
            List<string>? DeletePhoto,
            int LastPage,
            string? MapDataParam,
            string? LastAction)
        {
            //if (id != tStudent.fStuId)
            //{
            //    return NotFound();
            //}

            //因為有些錯誤會返回到原畫面所以乾脆直接在開頭就設ViewData
            await SetViewData();

            //Debug.WriteLine("Controller Edit 1 MapData : " + MapDataParam);
            if (!string.IsNullOrEmpty(MapDataParam))
                ViewData[ViewDataKeys.MapData] = Uri.UnescapeDataString(MapDataParam);
            //Debug.WriteLine("Controller Edit 2 MapData : " + Uri.UnescapeDataString(MapDataParam));
            ViewData[ViewDataKeys.AJAXAction] = LastAction ?? string.Empty;
            ViewData[ViewDataKeys.LastPage] = LastPage;

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

                var UploadPhotoResult = await VisitsServices.UploadPhoto(Visit, true, VisitPhotos, GetMemberID);
                if (UploadPhotoResult.Type == ResultType.Fail)
                {
                    Debug.WriteLine("Controller Edit UploadPhotoResult Error : " + UploadPhotoResult.Message);
                    ViewData["PhotoError"] = UploadPhotoResult.Message;
                    return NotFound(Visit);//不要回傳View，不然ajax會觸發success
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
                if (UpdatePhotoDataMsg.Type == ResultType.Fail)
                {
                    Debug.WriteLine("Controller Edit UpdatePhotoDataMsg Error : " + UpdatePhotoDataMsg.Message);
                    ViewData["PhotoError"] = UpdatePhotoDataMsg;
                    return NotFound(Visit);//不要回傳View，不然ajax會觸發success
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

                //switch (LastAction)
                //{
                //    case nameof(ShowVisitsOnMap):
                //        return RedirectToAction(nameof(ShowVisitsOnMap), new { MapDataParam = MapDataParam });
                //    case nameof(ShowVisitsOnList):
                //        return RedirectToAction(nameof(ShowVisitsOnList), new { Page = LastPage });
                //}

                //return NotFound();

                return View(Visit);
            }

            Shared.ShowModelState(ModelState);

            return View(Visit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="LastPage">上一頁</param>
        /// <param name="MapDataParam"></param>
        /// <param name="LastAction">上一個Action</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int VisitSN, int LastPage, string? MapDataParam, string? LastAction)
        {
            var Visit = await VisitsServices.GetVisit(VisitSN, SortPhotos: true);

            if (Visit == null)
            {
                Console.WriteLine($"VisitSN {VisitSN} not found");

                return NotFound();
            }

            SetGoogleMapsApiKey();

            if (!string.IsNullOrEmpty(MapDataParam))
            {
                //Debug.WriteLine(MapDataParam);

                //var MapData = JsonConvert.DeserializeObject<MapDataParam>(MapDataParam);
                //Debug.WriteLine("Details MapData : " + MapData);

                ViewData[ViewDataKeys.MapData] = Uri.UnescapeDataString(MapDataParam);//因為在View丟值過來時因為是帶在網頁上，需經過一次URL格式的轉換避免出錯，因此再次塞給View時需轉換回來才會是正確的Json格式
                //Debug.WriteLine("Controller Details MapData : " + Uri.UnescapeDataString(MapDataParam));
                //ViewData[ViewDataKeys.MapData] = MapData;
            }

            ViewData[ViewDataKeys.AJAXAction] = LastAction ?? string.Empty;
            ViewData[ViewDataKeys.LastPage] = LastPage;

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
        /// 取得回覆留言資料
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetContentsByViewComponent(int VisitSN)
        {
            Debug.WriteLine($"GetContentsByViewComponent VisitSN: {VisitSN}");

            return ViewComponent("VC_Comment", new { VisitSN = VisitSN });
        }

        /// <summary>
        /// 刪除到訪紀錄
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int VisitSN)
        {
            //Console.WriteLine($"Delete VisitSN : {VisitSN}");

            if (VisitSN < 0)
                return NotFound();

            var Result = await VisitsServices.DeleteVisit(VisitSN);
            if (Result.Type == ResultType.Fail)
                return NotFound();

            //刪除到訪紀錄照片
            //PhotoServices.DeletePhoto(Visit);

            //Context.Visits.Remove(Visit);
            //await Context.SaveChangesAsync();

            //SetGoogleMapsApiKey();
            return RedirectToAction(nameof(ShowVisitsOnList));
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task SetViewData()
        {
            ViewData[ViewDataKeys.CountryCode] = await CountriesServices.GetCountriesSelect();
            ViewData[ViewDataKeys.AnimeID] = await AnimesServices.GetAnimesSelect();
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
        async Task<Result> UpdatePhotoData(Visits Visit, List<VisitsPhotosDTO>? VisitPhotos)
        {
            if (VisitPhotos == null || VisitPhotos.Count == 0)
            {
                return new Result(Message: "沒有圖片資料更新");
            }

            //Debug.WriteLine($"UpdatePhotoData VisitPhotos Count : {VisitPhotos.Count()}");
            var UpdatePhotoData = VisitPhotos.FindAll(VP => VP.UploadFile == null);
            foreach (var PhotoData in UpdatePhotoData)
            {
                //Debug.WriteLine(VP.ToString());
                var Original = await Context.VisitsPhotos.FirstOrDefaultAsync(V => V.PhotoID == PhotoData.PhotoID);
                var OrderID = VisitPhotos.FindIndex(P => P.PhotoID == PhotoData.PhotoID);
                if (Original != null && (Original.Description != PhotoData.Description || Original.SortNumber != OrderID))//檢查資料是否有變動
                {
                    //Debug.WriteLine($"修改圖片資料 {Original.PhotoID}");
                    Original.Description = PhotoData.Description;
                    Original.SortNumber = OrderID;

                    Context.Update(Original);
                }
            }

            return new Result();
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
