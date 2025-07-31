using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Authorization;
using AniwalkServer.Data;

namespace AniwalkServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = Shared.Role_Admin)]
    public class MembersAdminController : Controller
    {
        private readonly AniwalkDBContext _context;

        public MembersAdminController(AniwalkDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 會員檢視
        /// </summary>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int Skip = 0, int Take = 0)
        {
            var Result = _context.Members
                .Include(M => M.Country)
                .Include(MS => MS.MemberStatus).ThenInclude(MSC => MSC.MemberStatusCode)
                .Skip(Skip);

            if (Take > 0)
                Result = Result.Take(Take);

            return View(await Result.ToListAsync());
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var members = await _context.Members
                .Include(MS => MS.MemberStatus).ThenInclude(MSC => MSC.MemberStatusCode)
                .FirstOrDefaultAsync(M => M.MemberID == id);
            if (members == null)
            {
                return NotFound();
            }

            SetViewData(members.CountryCode);

            //帶入此會員的帳號建立時間，避免更新資料時被帶入當下時間
            ViewData["CreatedDate"] = members.CreatedDate;
            ViewData["MemberStatusCode"] = new SelectList(
                _context.MemberStatusCode,
                nameof(MemberStatusCode.StatusCode),
                nameof(MemberStatusCode.StatusName),
                members.MemberStatus?.StatusCode);

            return View(members);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,Name,Email,CreatedDate,CountryCode,MemberStatus")] Members members)
        {
            if (id != members.MemberID)
            {
                return NotFound();
            }

            Console.WriteLine("MemberStatus : " + members.MemberStatus.StatusCode);

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

            Shared.ShowModelState(ModelState);

            return View(members);
        }

        private bool MembersExists(string id)
        {
            return _context.Members.Any(e => e.MemberID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryCode"></param>
        public void SetViewData(string? CountryCode = null)
        {
            ViewData["CountryCode"] = new SelectList(_context.Countries, "CountryCode", "CountryName", CountryCode);
        }
    }
}