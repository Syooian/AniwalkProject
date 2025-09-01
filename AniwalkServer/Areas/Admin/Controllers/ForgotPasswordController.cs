using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models.ForgotPassword;
using AniwalkServer.Services;
using AniwalkServer.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AniwalkServer.Admin.Controllers
{
    [Area(Shared.Role_Admin), Authorize(Roles = Shared.Role_Admin)]
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
        LoginServices LoginServices;
        /// <summary>
        /// 
        /// </summary>
        MailServices MailServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ForgotPasswordServices"></param>
        /// <param name="MembersServices"></param>
        /// <param name="LoginServices"></param>
        /// <param name="MailServices"></param>
        public ForgotPasswordController(AniwalkDBContext context, ForgotPasswordServices ForgotPasswordServices, MembersServices MembersServices, LoginServices LoginServices, MailServices MailServices)
        {
            _context = context;
            this.ForgotPasswordServices = ForgotPasswordServices;
            this.MembersServices = MembersServices;
            this.LoginServices = LoginServices;
            this.MailServices = MailServices;
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
