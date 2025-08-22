using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using AniwalkServer.Services;
using System.Diagnostics;

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

            return View(Result);
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var announcements = await AnnouncementsServices.GetAnnouncement(id);

            if (announcements == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(int id)
        {
            var announcements = await AnnouncementsServices.GetAnnouncement(id);

            if (announcements == null)
            {
                return NotFound();
            }

            return View(announcements);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SN,Title,Content")] Announcements announcements)
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
                return RedirectToAction(nameof(Index));
            }
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
