using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    public class LoginServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public LoginServices(AniwalkDBContext Context) : base(Context) { }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="NewPassword">新的密碼</param>
        /// <returns></returns>
        public async Task<Result> ChangePassword(string MemberID, string NewPassword)
        {
            var Login = await GetLoginByMemberID(MemberID);
            if (Login == null)
            {
                return new Result(ResultType.Fail, "找不到帳號");
            }

            try
            {
                Login.Password = NewPassword;
                Context.Login.Update(Login);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ChangePassword Error : {ex.Message}");
                return new Result(ResultType.Fail, "更新密碼時發生錯誤");
            }

            return new Result();
        }

        /// <summary>
        /// 取得會員帳密
        /// </summary>
        /// <param name="Login">登入畫面輸入的帳密</param>
        /// <returns></returns>
        public async Task<Login> GetLogin(Login Login)
        {
            return await Context.Login.FirstOrDefaultAsync(M => M.Account == Login.Account && M.Password == Login.Password);
        }

        /// <summary>
        /// 取得會員帳密
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public async Task<Login> GetLoginByAccount(string Account)
        {
            return await Context.Login.FirstOrDefaultAsync(M => M.Account == Account);
        }

        /// <summary>
        /// 取得會員帳密
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<Login> GetLoginByMemberID(string MemberID)
        {
            return await Context.Login.FirstOrDefaultAsync(M => M.MemberID == MemberID);
        }

        /// <summary>
        /// 檢查密碼
        /// </summary>
        /// <param name="ComparePassword">原始密碼</param>
        /// <param name="InputPassword">輸入的密碼</param>
        /// <returns></returns>
        public bool CheckPassword(string ComparePassword, string InputPassword)
        {
            return ComparePassword == InputPassword;
        }
    }
}
