using AniwalkServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        /// <param name="context"></param>
        public LoginController(AniwalkDBContext context)
        {
            Context = context;
        }

        /// <summary>
        /// 身份驗證方案名稱
        /// </summary>
        public const string AuthenticationScheme = "UserLogin"; // 定義一個常數用於身份驗證方案名稱

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
            var User = Context.Login.FirstOrDefault(u => u.Account == Login.Account && u.Password == Login.Password);
            if (User != null)
            {
                var Member = await Context.Members.FirstOrDefaultAsync(M => M.MemberID == User.MemberID);
                //var MemberRole = await Context.MemberRoles.FirstOrDefaultAsync(MR => MR.RoleID == Member.RoleID);

                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Member.Name),
                    new Claim(ClaimTypes.Role, ((RoleEnum)Member.RoleID).ToString()),
                    new Claim(ClaimTypes.NameIdentifier, Member.MemberID)
                };

                var ClaimsIdentity = new ClaimsIdentity(Claims, AuthenticationScheme);
                var ClaimsPrincipal = new ClaimsPrincipal(ClaimsIdentity);

                await HttpContext.SignInAsync(AuthenticationScheme, ClaimsPrincipal); //把資料寫入 Cookie 進行登入狀態管理

                return RedirectToAction("Index", "Home"); // 登入成功後導向到 BooksManage 的 Index 頁面
            }

            ViewData["Error"] = "帳號或密碼錯誤，請重新輸入";
            return View(Login);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [Authorize] //標註需要登入才能使用
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(AuthenticationScheme); //登出時清除 Cookie
            return RedirectToAction("Index", "Home"); // 登出後導向到 Home 的 Index 頁面
        }
    }
}
