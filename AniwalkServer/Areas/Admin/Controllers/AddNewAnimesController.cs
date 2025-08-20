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
using AniwalkServer.Data;
using AniwalkServer.Services;

namespace AniwalkServer.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area(Shared.Role_Admin), Authorize(Roles = Shared.Role_Admin)]
    public class AddNewAnimesController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        AddNewAnimesServices AddNewAnimesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="AddNewAnimesServices"></param>
        public AddNewAnimesController(AniwalkDBContext Context, AddNewAnimesServices AddNewAnimesServices)
        {
            this.Context = Context;
            this.AddNewAnimesServices = AddNewAnimesServices;
        }

        // GET: AddNewAnimes
        /// <summary>
        /// 檢視新增動畫建議
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var Result = await AddNewAnimesServices.GetAddNewAnimes();

            return View(Result);
        }

        // GET: AddNewAnimes/Edit/5
        /// <summary>
        /// 編輯新增動畫建議
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Shared.Role_Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var addNewAnime = await AddNewAnimesServices.GetAddNewAnime(id);
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

                    Context.Update(addNewAnime);

                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddNewAnimesServices.IsAddNewAnimeExists(addNewAnime.SN))
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
    }
}
