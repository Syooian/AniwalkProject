using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
        /// 註冊會員
        /// </summary>
        /// <returns></returns>
        // GET: Members/Create
        //[Authorize(Roles = Shared.Role_Guest)]
        public async Task<IActionResult> Create()
        {
            await SetViewData();

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = Shared.Role_Guest)]
        public async Task<IActionResult> Create([Bind("Name,Email,CountryCode,Account,Password,PasswordConfirm")] RegistDTO RegistData)
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
                async Task<ViewResult> ReturnView(string Message)
                {
                    Debug.WriteLine(Message);
                    ViewData["Error"] = Message;
                    await SetViewData();
                    return View(RegistData);
                }

                #region 檢查輸入的會員名稱是否已被使用
                //Debug.WriteLine($"Account : {RegistData.Name}");
                var CheckName = await MembersServices.GetMemberByName(RegistData.Name);
                if (CheckName != null)
                {
                    return await ReturnView("此會員名稱已被使用。");
                }
                #endregion

                #region 檢查輸入的電子郵件是否已被使用
                //Debug.WriteLine($"Email : {RegistData.Email}");
                var CheckEmail = await MembersServices.GetMemberByEmail(RegistData.Email);
                if (CheckEmail != null)
                {
                    return await ReturnView("此電子郵件已被使用。");
                }
                #endregion

                #region 檢查輸入的帳號是否已被使用
                //Debug.WriteLine($"Email : {RegistData.Account}");
                var CheckAccount = await MembersServices.GetMemberByAccount(RegistData.Account);
                if (CheckAccount != null)
                {
                    return await ReturnView("此帳號名稱已被使用。");
                }
                #endregion

                //創建新會員
                var CreateNewMemberResult = await MembersServices.CreateNewMember(RegistData);
                if (CreateNewMemberResult.Type == ResultType.Fail)
                {
                    return await ReturnView(CreateNewMemberResult.Message);
                }

                return RedirectToAction(nameof(LoginController.Login), nameof(LoginController.Login));
            }

            #region 檢查模型驗證
            Shared.ShowModelState(ModelState);
            #endregion

            await SetViewData(RegistData.CountryCode);

            return View(RegistData);
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
        public async Task<IActionResult> Edit(string id, [Bind("MemberID,Name,Email,CreatedDate,CountryCode,RoleID")] Members members)
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
        public async Task SetViewData(string? CountryCode = null)
        {
            ViewData[ViewDataKeys.CountryCode] = await CountriesServices.GetCountriesSelect(CountryCode);
        }
    }
}