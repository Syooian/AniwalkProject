using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;

namespace AniwalkServer.Controllers
{
    public class MembersController : Controller
    {
        private readonly AniwalkDBContext _context;

        public MembersController(AniwalkDBContext context)
        {
            _context = context;
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["CountryCode"] = new SelectList(_context.Countries, "CountryCode", "CountryCode");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberID,Name,Email,CreatedDate,CountryCode")] Members members)
        {
            if (ModelState.IsValid)
            {
                _context.Add(members);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryCode"] = new SelectList(_context.Countries, "CountryCode", "CountryCode", members.CountryCode);
            return View(members);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var members = await _context.Members.FindAsync(id);
            if (members == null)
            {
                return NotFound();
            }
            ViewData["CountryCode"] = new SelectList(_context.Countries, "CountryCode", "CountryCode", members.CountryCode);
            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,Name,Email,CreatedDate,CountryCode")] Members members)
        {
            if (id != members.MemberID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(members);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembersExists(members.MemberID))
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
            ViewData["CountryCode"] = new SelectList(_context.Countries, "CountryCode", "CountryCode", members.CountryCode);
            return View(members);
        }

        private bool MembersExists(string id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }
    }
}
