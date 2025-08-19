using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;
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
            var Login = await GetLogin(MemberID);
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
        /// 取得帳密
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<Login> GetLogin(string MemberID)
        {
            return await Context.Login.FirstOrDefaultAsync(M => M.MemberID == MemberID);
        }
    }
}
