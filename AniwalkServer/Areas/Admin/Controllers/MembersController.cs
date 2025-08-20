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
using AniwalkServer.Services;
using System.Diagnostics;

namespace AniwalkServer.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area(Shared.Role_Admin)]
    [Authorize(Roles = Shared.Role_Admin)]
    public class MembersController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext _context;
        #region Services
        /// <summary>
        /// 
        /// </summary>
        readonly MembersServices MembersServices;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MembersServices"></param>
        public MembersController(AniwalkDBContext context, MembersServices MembersServices)
        {
            _context = context;
            #region Services
            this.MembersServices = MembersServices;
            #endregion
        }

        /// <summary>
        /// 會員列表
        /// </summary>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int Skip = 0, int Take = 0)
        {
            var Result = await MembersServices.GetMembers();

            return View(Result);
        }

        // GET: Members/Edit/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string MemberID)
        {
            if (MemberID == null)
            {
                return NotFound();
            }

            var Member = await MembersServices.GetMember(MemberID);

            if (Member == null)
            {
                return NotFound();
            }

            //SetViewData(members.CountryCode);

            //帶入此會員的帳號建立時間，避免更新資料時被帶入當下時間
            ViewData["CreatedDate"] = Member.CreatedDate;

            return View(Member);
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
                    if (!MembersServices.IsMembersExists(members.MemberID))
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

            return View(members);
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