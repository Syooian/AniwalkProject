using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AniwalkServer.Controllers
{
    public class AddNewAnimesController : Controller
    {
        private readonly AniwalkDBContext _context;

        public AddNewAnimesController(AniwalkDBContext context)
        {
            _context = context;
        }

        // GET: AddNewAnimes
        /// <summary>
        /// 檢視新增動畫建議
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Shared.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AddNewAnimes.ToListAsync());
        }

        // GET: AddNewAnimes/Create
        /// <summary>
        /// 建立新的新增動畫建議
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Shared.Role_Member)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddNewAnimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Create([Bind("SN,AnimeTitle,AddDate,Status,CloseDate,Note")] AddNewAnime addNewAnime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addNewAnime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addNewAnime);
        }

        // GET: AddNewAnimes/Edit/5
        /// <summary>
        /// 編輯新增動畫建議
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Shared.Role_Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addNewAnime = await _context.AddNewAnimes.FindAsync(id);
            if (addNewAnime == null)
            {
                return NotFound();
            }

            //// 將 Enum 轉換為 SelectList
            //ViewData["Status"] = new SelectList(Enum.GetValues(typeof(AddNewAnimeStatusEnum))
            //    .Cast<AddNewAnimeStatusEnum>()
            //    .Select(e => new { Value = (int)e, Text =addNewAnime.GetStatusDisplayName( e.get .ToString() }), "Value", "Text");

            return View(addNewAnime);
        }

        // POST: AddNewAnimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 編輯新增動畫建議
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addNewAnime"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Shared.Role_Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("SN,AnimeTitle,AddDate,Status,CloseDate,Note")] AddNewAnime addNewAnime)
        {
            if (id != addNewAnime.SN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    switch (addNewAnime.Status)
                    {
                        case AddNewAnimeStatusEnum.AgreeToAdd:
                        case AddNewAnimeStatusEnum.Disagree:
                            addNewAnime.CloseDate = DateTime.Now;
                            break;
                    }

                    _context.Update(addNewAnime);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddNewAnimeExists(addNewAnime.SN))
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
            return View(addNewAnime);
        }

        private bool AddNewAnimeExists(int id)
        {
            return _context.AddNewAnimes.Any(e => e.SN == id);
        }
    }
}
