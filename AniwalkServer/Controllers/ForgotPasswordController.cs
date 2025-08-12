using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Data;
using AniwalkServer.Models.ForgotPassword;
using AniwalkServer.Services;

namespace AniwalkServer.Controllers
{
    public class ForgotPasswordController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        ForgotPasswordServices ForgotPasswordServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ForgotPasswordServices"></param>
        public ForgotPasswordController(AniwalkDBContext context, ForgotPasswordServices ForgotPasswordServices)
        {
            _context = context;
            this.ForgotPasswordServices = ForgotPasswordServices;
        }

        // GET: ForgotPassword
        public async Task<IActionResult> Index()
        {
            var aniwalkDBContext = _context.ForgotPassword.Include(f => f.Member);
            return View(await aniwalkDBContext.ToListAsync());
        }

        // GET: ForgotPassword/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forgotPassword = await _context.ForgotPassword
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.SN == id);
            if (forgotPassword == null)
            {
                return NotFound();
            }

            return View(forgotPassword);
        }

        // GET: ForgotPassword/Create
        public IActionResult Create()
        {
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID");
            return View();
        }

        // POST: ForgotPassword/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SN,VerifyCodeExpiryDate,CreatedDate,VerifyCode,MemberID")] ForgotPassword forgotPassword)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forgotPassword);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", forgotPassword.MemberID);
            return View(forgotPassword);
        }

        // GET: ForgotPassword/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forgotPassword = await _context.ForgotPassword.FindAsync(id);
            if (forgotPassword == null)
            {
                return NotFound();
            }
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", forgotPassword.MemberID);
            return View(forgotPassword);
        }

        // POST: ForgotPassword/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SN,VerifyCodeExpiryDate,CreatedDate,VerifyCode,MemberID")] ForgotPassword forgotPassword)
        {
            if (id != forgotPassword.SN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forgotPassword);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForgotPasswordExists(forgotPassword.SN))
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
            ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "MemberID", forgotPassword.MemberID);
            return View(forgotPassword);
        }

        // GET: ForgotPassword/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forgotPassword = await _context.ForgotPassword
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.SN == id);
            if (forgotPassword == null)
            {
                return NotFound();
            }

            return View(forgotPassword);
        }

        // POST: ForgotPassword/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forgotPassword = await _context.ForgotPassword.FindAsync(id);
            if (forgotPassword != null)
            {
                _context.ForgotPassword.Remove(forgotPassword);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForgotPasswordExists(int id)
        {
            return _context.ForgotPassword.Any(e => e.SN == id);
        }
    }
}
