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

namespace AniwalkServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = Shared.Role_Member)]
    public class AddNewAnimesController : Controller
    {
        private readonly AniwalkDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        AddNewAnimesServices AddNewAnimesServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="AddNewAnimesServices"></param>
        public AddNewAnimesController(AniwalkDBContext context, AddNewAnimesServices AddNewAnimesServices)
        {
            _context = context;
            this.AddNewAnimesServices = AddNewAnimesServices;
        }

        // GET: AddNewAnimes/Create
        /// <summary>
        /// 建立新的新增動畫建議
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddNewAnimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SN,AnimeTitle,AddDate,Status,CloseDate,Note")] AddNewAnime addNewAnime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(addNewAnime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(addNewAnime);
        }
    }
}
