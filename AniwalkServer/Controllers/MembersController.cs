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

namespace AniwalkServer.Controllers
{
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
        /// 註冊會員
        /// </summary>
        /// <returns></returns>
        // GET: Members/Create
        //[Authorize(Roles = Shared.Role_Guest)]
        public IActionResult Create()
        {
            SetViewData();

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = Shared.Role_Guest)]
        public async Task<IActionResult> Create([Bind("MemberID,Name,Email,CountryCode")] Members members, /*[Bind("Account,Password")]*/ Login Login)
        {
            #region Dev
            //try
            //{
            //    Console.WriteLine($"{members.Name} {members.Email} {members.CountryCode}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Member Error : {ex.Message}");
            //}

            //try
            //{
            //    Console.WriteLine($"Login : {Login.Account}&{Login.Password}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Login Error : {ex.Message}");
            //}

            //try
            //{
            //    Console.WriteLine($"Login : {members.Login.Account}&{members.Login.Password}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Login Error : {ex.Message}");
            //}
            #endregion

            if (ModelState.IsValid)
            {
                #region 檢查輸入的會員名稱是否已被使用
                if (_context.Members.Any(m => m.Name == members.Name))
                {
                    ViewData["Error"] = "此會員名稱已被使用。";
                    SetViewData();
                    return View(members);
                }
                #endregion

                #region 檢查輸入的電子郵件是否已被使用
                if (_context.Members.Any(m => m.Email == members.Email))
                {
                    ViewData["Error"] = "此電子郵件已被使用。";
                    SetViewData();
                    return View(members);
                }
                #endregion

                //生成MemberID並檢查是否重複
                while (true)
                {
                    var NewMemberID = new Random().Next(0, 999999999).ToString("D10"); // 生成隨機的10位數會員ID
                    if (!_context.Members.Any(m => m.MemberID == NewMemberID)) // 檢查是否已存在相同的會員ID
                    {
                        members.MemberID = NewMemberID; // 如果不存在，則使用這個ID
                        break;
                    }
                }

                members.CreatedDate = DateTime.Now; // 設定創建日期為當前時間

                //將Login資料與Members關聯
                Login.MemberID = members.MemberID; // 設定Login的MemberID為新生成的會員ID

                members.RoleID = (int)RoleEnum.Member; // 設定會員角色為一般會員

                members.MemberStatus = new MemberStatus();//新增會員狀態

                _context.Add(members);
                _context.Add(Login);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
            }

            #region 檢查模型驗證
            //foreach (var key in ModelState.Keys)
            //{
            //    var errors = ModelState[key].Errors;
            //    if (errors.Any())
            //    {
            //        Console.WriteLine($"Key: {key}, Errors: {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
            //    }
            //}
            //Console.WriteLine("ModelState is invalid. Errors: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            #endregion

            SetViewData(members.CountryCode);

            return View(members);
        }

        // GET: Members/Edit/5
        [Authorize(Roles = Shared.Role_Member)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Member = await MembersServices.GetMember(id);

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
        [Authorize(Roles = Shared.Role_Member)]
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

                return RedirectToAction(nameof(Details), new { MemberID = members.MemberID });
            }

            return View(members);
        }

        /// <summary>
        /// 個人資訊檢視
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string MemberID)
        {
            var Member = await MembersServices.GetMember(MemberID);

            if (Member == null)
            {
                return NotFound();
            }

            return View(Member);
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