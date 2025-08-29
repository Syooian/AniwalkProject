using AniwalkServer.Data;
using AniwalkServer.Models;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AniwalkServer.Admin.Controllers
{
    [Area(Shared.Role_Admin), Authorize(Roles = Shared.Role_Admin)]
    public class AnnouncementsController : Controller
    {
        private readonly AniwalkDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        readonly AnnouncementsServices AnnouncementsServices;

        public AnnouncementsController(AniwalkDBContext context, AnnouncementsServices AnnouncementsServices)
        {
            _context = context;
            this.AnnouncementsServices = AnnouncementsServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int Page = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        {
            var Result = await AnnouncementsServices.GetAnnouncements(Page, PageSize);

            if (Result == null)
            {
                return NotFound();
            }

            ViewData[ViewDataKeys.LastPage] = Page;

            return View(Result);
        }

        // GET: Announcements/Details/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="LastPage"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int id, int LastPage)
        {
            var announcements = await AnnouncementsServices.GetAnnouncement(id);

            if (announcements == null)
            {
                return NotFound();
            }

            ViewData[ViewDataKeys.LastPage] = LastPage;

            return View(announcements);
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SN,Title,Content")] Announcements announcements)
        {
            if (ModelState.IsValid)
            {
                //announcements.CreatedDate = DateTime.Now; // 設定建立日期
                //似乎不需要特地寫，可能是因為已在Model指定預設值為Datetime.Now

                _context.Add(announcements);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcements);
        }

        // GET: Announcements/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int LastPage)
        {
            var announcements = await AnnouncementsServices.GetAnnouncement(id);

            if (announcements == null)
            {
                return NotFound();
            }

            ViewData[ViewDataKeys.LastPage] = LastPage;

            return View(announcements);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SN,Title,Content")] Announcements announcements, int LastPage)
        {
            if (id != announcements.SN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcements);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementsServices.IsAnnouncementsExists(announcements.SN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index), new { Page = LastPage });
            }

            ViewData[ViewDataKeys.LastPage] = LastPage;

            return View(announcements);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcements = await _context.Announcements.FindAsync(id);
            if (announcements != null)
            {
                _context.Announcements.Remove(announcements);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
