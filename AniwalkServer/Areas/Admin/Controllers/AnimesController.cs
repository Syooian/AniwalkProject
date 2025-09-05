using AniwalkServer.Data;
using AniwalkServer.Models;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Animes.ToListAsync());
        }

        // GET: Admin/Animes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animes = await _context.Animes
                .FirstOrDefaultAsync(m => m.AnimeID == id);
            if (animes == null)
            {
                return NotFound();
            }

            return View(animes);
        }

        // GET: Admin/Animes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Animes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimeID,Title,HeaderPhoto,Description,CreatedDate")] Animes animes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animes);
        }

        // GET: Admin/Animes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animes = await _context.Animes.FindAsync(id);
            if (animes == null)
            {
                return NotFound();
            }
            return View(animes);
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
                    if (!AnimesExists(animes.AnimeID))
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

        // GET: Admin/Animes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animes = await _context.Animes
                .FirstOrDefaultAsync(m => m.AnimeID == id);
            if (animes == null)
            {
                return NotFound();
            }

            return View(animes);
        }

        // POST: Admin/Animes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var animes = await _context.Animes.FindAsync(id);
            if (animes != null)
            {
                _context.Animes.Remove(animes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimesExists(string id)
        {
            return _context.Animes.Any(e => e.AnimeID == id);
        }
    }
}
