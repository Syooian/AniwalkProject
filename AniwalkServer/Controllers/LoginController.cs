using AniwalkServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AniwalkServer.Data;
using AniwalkServer.Services;
using System.Diagnostics;

namespace AniwalkServer.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        readonly LoginServices LoginServices;
        /// <summary>
        /// 
        /// </summary>
        readonly MembersServices MembersServices;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MembersServices"></param>
        /// <param name="LoginServices"></param>
        public LoginController(AniwalkDBContext context, MembersServices MembersServices, LoginServices LoginServices)
        {
            Context = context;

            this.LoginServices = LoginServices;
            this.MembersServices = MembersServices;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]//標註不須登入
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Login Login)
        {
            #region
            ////先找有沒有此會員
            //var R = await LoginServices.GetLoginByAccount(Login.Account);
            //if (R == null)
            //{
            //    ViewData["Error"] = "無此帳號";
            //    return View(Login);
            //}

            ////確認密碼
            //var CheckPassword = LoginServices.CheckPassword(R.Password, Login.Password);
            //if (!CheckPassword)
            //{

            //}
            #endregion

            var Result = await LoginServices.GetLogin(Login);
            if (Result == null)
            {
                ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";
                Debug.WriteLine("Login Result is null");
                return View(Login);
            }

            var Member = await MembersServices.GetMember(Result.MemberID);
            if (Member == null)
            {
                ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";
                Debug.WriteLine("Login Member is null");
                return View(Login);
            }

            //檢查會員狀態
            if (Member.MemberStatus.StatusCode != 0)
            {
                ViewData["Error"] = "帳號" + Member.MemberStatus.MemberStatusCode.StatusName;
                return View(Login);
            }

            var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Member.Name),
                    new Claim(ClaimTypes.Role, ((RoleEnum)Member.RoleID).ToString()),
                    new Claim(ClaimTypes.NameIdentifier, Member.MemberID)
                };

            var ClaimsIdentity = new ClaimsIdentity(Claims, Shared.AuthenticationScheme);
            var ClaimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);

            await HttpContext.SignInAsync(Shared.AuthenticationScheme, ClaimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理

            return RedirectToAction("Index", "Home"); // 登入成功後導向到 BooksManage 的 Index 頁面

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [Authorize] //標註需要登入才能使用
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Shared.AuthenticationScheme); //登出時清除 Cookie
            return RedirectToAction("Index", "Home"); // 登出後導向到 Home 的 Index 頁面
        }
    }
}
