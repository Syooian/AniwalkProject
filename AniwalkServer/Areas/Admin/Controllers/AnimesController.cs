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

            if (Result == null)
                return NotFound();

            SetViewData(Page);

            return View(Result);
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
        public async Task<IActionResult> Create([Bind("Title,HeaderPhoto,Description")] Animes Anime)
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
                _context.Add(Anime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(string id, [Bind("AnimeID,Title,HeaderPhoto,Description,CreatedDate")] Animes animes)
        {
            if (id != animes.AnimeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var Result = await AnimesServices.IsAnimeExists(animes.AnimeID);

                    if (!Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(animes);
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
