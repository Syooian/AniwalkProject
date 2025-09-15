using AniwalkServer.Data;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AniwalkServer.Areas.Admin.Controllers
{
    /// <summary>
    /// 動畫管理
    /// </summary>
    [Area(Shared.Role_Admin), Authorize(Roles = Shared.Role_Admin)]
    public class AnimesController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        readonly AnimesServices AnimesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="AnimesServices"></param>
        public AnimesController(AniwalkDBContext context, AnimesServices AnimesServices)
        {
            _context = context;
            this.AnimesServices = AnimesServices;
        }

        // GET: Admin/Animes
        /// <summary>
        /// 動畫清單
        /// </summary>
        /// <param name="AnimesParam"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(AnimesParam? AnimesParam, int Page = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        {
            var Result = await AnimesServices.GetAnimes(AnimesParam, Page, PageSize);

            ViewData["AnimeID"] = await AnimesServices.GetAnimeIDsSelect(IncludeDisabled: true);
            ViewData["AnimeTitle"] = await AnimesServices.GetAnimeTitlesSelect(IncludeDisabled: true);

            if (Result == null)
                return NotFound();

            SetViewData(Page);

            // 判斷是否為 AJAX 請求
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // 回傳部分視圖（只渲染清單）
                return PartialView("Index.List", Result); //需只渲染清單
            }
            else//一般頁面載入
            {
                return View(Result);
            }
        }

        // GET: Admin/Animes/Details/5
        /// <summary>
        /// 動畫檢視
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LastPage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(string id, int LastPage)
        {
            if (id == null)
                return NotFound();

            var Result = await AnimesServices.GetAnime(id);
            if (Result == null)
                return NotFound();

            SetViewData(LastPage);

            return View(Result);
        }

        // GET: Admin/Animes/Create
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LastPage"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create(int? LastPage)
        {
            SetViewData((int)LastPage);

            return View();
        }

        // POST: Admin/Animes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,HeaderPhoto,Description")] Animes Anime, IFormFile? HeaderPhoto)
        {
            //移除AnimeID的模型驗證，由後端手動增加
            ModelState.Remove(nameof(Animes.AnimeID));

            var CheckExists = await AnimesServices.IsAnimeExists(Anime.Title);
            if (CheckExists)
            {
                ViewData["Err"] = "該動畫已存在";
                return View(Anime);
            }

            Anime.CreatedDate = DateTime.Now;
            Anime.AnimeID = await AnimesServices.GetNewAnimeID();

            if (ModelState.IsValid)
            {
                using var Transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Add(Anime);
                    await _context.SaveChangesAsync();

                    //圖片上傳
                    var UploadHeaderPhotoResult = await AnimesServices.UploadHeaderPhoto(Anime.AnimeID, HeaderPhoto);
                    if (UploadHeaderPhotoResult.Type == ResultType.Success)
                    {
                        await Transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("新增動畫失敗 EX : " + ex.Message);
                }

                ViewData["Err"] = "動畫新增失敗";
                await Transaction.RollbackAsync();
                return View(Anime);
            }

            //Shared.ShowModelState(ModelState);

            return View(Anime);
        }

        // GET: Admin/Animes/Edit/5
        /// <summary>
        /// 編輯動畫
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LastPage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id, int LastPage)
        {
            if (id == null)
                return NotFound();

            var Result = await AnimesServices.GetAnime(id);
            if (Result == null)
                return NotFound();

            SetViewData(LastPage);

            return View(Result);
        }

        // POST: Admin/Animes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AnimeID,Title,HeaderPhoto,Description,CreatedDate")] Animes Anime, IFormFile? HeaderPhoto, string? DeleteHeaderPhoto)
        {
            if (id != Anime.AnimeID)
            {
                return NotFound();
            }

            void SetError()
            {
                ViewData["Err"] = "動畫編輯失敗";
            }

            if (ModelState.IsValid)
            {
                using var Transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Update(Anime);
                    await _context.SaveChangesAsync();

                    //刪除圖片
                    if (!string.IsNullOrEmpty(DeleteHeaderPhoto) && DeleteHeaderPhoto == "1")
                    {
                        var DeleteHeaderPhotoResult = AnimesServices.DeleteHeaderPhoto(Anime.AnimeID);
                        if (DeleteHeaderPhotoResult.Type == ResultType.Fail)
                        {
                            SetError();
                            await Transaction.RollbackAsync();
                            Debug.WriteLine("DeleteHeaderPhoto EX : " + DeleteHeaderPhotoResult.Message);
                            return View(Anime);
                        }
                    }
                    else
                    {
                        //刪除舊的Header圖 (因為可能會有同檔名但是不同副檔名的情況)
                        var DeleteHeaderPhotoResult = AnimesServices.DeleteHeaderPhoto(Anime.AnimeID);
                        if (DeleteHeaderPhotoResult.Type == ResultType.Fail)
                        {
                            SetError();
                            await Transaction.RollbackAsync();
                            Debug.WriteLine("DeleteHeaderPhotoOnUpload EX : " + DeleteHeaderPhotoResult.Message);
                            return View(Anime);
                        }

                        //圖片上傳
                        var UploadHeaderPhotoResult = await AnimesServices.UploadHeaderPhoto(Anime.AnimeID, HeaderPhoto);
                        if (UploadHeaderPhotoResult.Type == ResultType.Fail)
                        {
                            SetError();
                            await Transaction.RollbackAsync();
                            Debug.WriteLine("UploadHeaderPhoto EX : " + UploadHeaderPhotoResult.Message);
                            return View(Anime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetError();
                    await Transaction.RollbackAsync();
                    Debug.WriteLine("Edit EX : " + ex.Message);
                    return View(Anime);
                }

                await Transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(Anime);
        }

        // POST: Admin/Animes/Disable/5
        /// <summary>
        /// 停用動畫
        /// </summary>
        /// <param name="AnimeID">動畫ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable(string AnimeID)
        {
            var Anime = await AnimesServices.GetAnime(AnimeID);
            if (Anime == null)
                return NotFound();

            Anime.DisabledDate = DateTime.Now;

            _context.Animes.Update(Anime);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Page"></param>
        void SetViewData(int Page)
        {
            ViewData[ViewDataKeys.LastPage] = Page;
        }
    }
}
