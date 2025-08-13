using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models.ForgotPassword;
using AniwalkServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        MembersServices MembersServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ForgotPasswordServices"></param>
        /// <param name="MembersServices"></param>
        public ForgotPasswordController(AniwalkDBContext context, ForgotPasswordServices ForgotPasswordServices, MembersServices MembersServices)
        {
            _context = context;
            this.ForgotPasswordServices = ForgotPasswordServices;
            this.MembersServices = MembersServices;
        }

        // GET: ForgotPassword
        [Authorize(Roles = Shared.Role_Admin)]
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
            return View();
        }

        // POST: ForgotPassword/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("SN,VerifyCodeExpiryDate,CreatedDate,VerifyCode,MemberID")] ForgotPassword forgotPassword)
        public async Task<IActionResult> Create([Bind("Email,Phase,VerifyCode")] ForgotPasswordDTO FP_DTO)
        {
            switch (FP_DTO.Phase)
            {
                case ForgotPasswordDTOPhase.Email://檢查Email
                    {
                        //檢查此會員是否存在
                        var Member = await MembersServices.GetMemberByEmail(FP_DTO.Email);
                        if (Member == null)
                        {
                            //ModelState.AddModelError("MemberID", "指定的會員不存在。");
                            SetErrorMessage("會員不存在。");
                            return View(FP_DTO);
                        }

                        //檢查驗證碼是否到期
                        if (await ForgotPasswordServices.IsForgotPasswordExpired(Member))
                        {
                            //新增表單
                            var Result = await ForgotPasswordServices.CreateForgotPassword(Member);
                            if (Result != "")
                            {
                                return View(FP_DTO);
                            }

                            FP_DTO.Phase = ForgotPasswordDTOPhase.VerifyCode; //設定為第二階段，輸入驗證碼

                            return View(FP_DTO);
                        }
                        else
                        {
                            SetErrorMessage("驗證碼尚未過期，請稍後再試。");
                            return View(FP_DTO);
                        }
                    }
                case ForgotPasswordDTOPhase.VerifyCode://檢查驗證碼
                    {
                        //檢查驗證碼是否正確
                        if (!await ForgotPasswordServices.IsForgotPasswordExists(FP_DTO.VerifyCode))
                        {
                            SetErrorMessage("驗證碼錯誤");
                            return View(FP_DTO);
                        }

                        //回到登入畫面讓使用者登入
                        return RedirectToAction("Login", "Login");
                    }
            }

            return View();
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
                    if (!ForgotPasswordServices.IsForgotPasswordExists(forgotPassword.SN))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        void SetErrorMessage(string Message)
        {
            ViewData["ErrorMessage"] = Message;
        }
    }
}
