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
        /// <summary>
        /// 
        /// </summary>
        readonly CountriesServices CountriesServices;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MembersServices"></param>
        /// <param name="CountriesServices"></param>
        public MembersController(AniwalkDBContext context, MembersServices MembersServices, CountriesServices CountriesServices)
        {
            _context = context;
            #region Services
            this.MembersServices = MembersServices;
            this.CountriesServices = CountriesServices;
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

            var MemberStatus = await MembersServices.GetMemberStatus(MemberID);

            if (MemberStatus == null)
            {
                return NotFound();
            }

            //SetViewData(members.CountryCode);

            ViewData[ViewDataKeys.MemberStatusCode] = await MembersServices.GetMemberStatusCodeSelect(MemberStatus.StatusCode);

            //帶入此會員的帳號建立時間，避免更新資料時被帶入當下時間
            //ViewData["CreatedDate"] = Member.CreatedDate;

            return View(MemberStatus);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberStatus"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string MemberID, [Bind("MemberID,StatusCode,Note")] MemberStatus MemberStatus)
        {
            if (MemberID != MemberStatus.MemberID)
            {
                return NotFound();
            }

            //Console.WriteLine("MemberStatus : " + members.MemberStatus.StatusCode);

            if (ModelState.IsValid)
            {
                try
                {
                    MemberStatus.UpdateDate = DateTime.Now;//資料庫不會反應時間，待修

                    _context.Update(MemberStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembersServices.IsMembersExists(MemberStatus.MemberID))
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

            return View(MemberStatus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CountryCode"></param>
        public async Task SetViewData(string? CountryCode = null)
        {
            ViewData[ViewDataKeys.CountryCode] = await CountriesServices.GetCountriesSelect(CountryCode);
        }
    }
}