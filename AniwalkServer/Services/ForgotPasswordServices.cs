using AniwalkServer.Data;
using AniwalkServer.Models;
using AniwalkServer.Models.ForgotPassword;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    public class ForgotPasswordServices
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ForgotPasswordServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 查找忘記密碼表單是否已過期
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public async Task<bool> IsForgotPasswordExpired(Members Member)
        {
            try
            {
                var Result = await Context.ForgotPassword
                    .Where(M => M.MemberID == Member.MemberID)
                    .OrderByDescending(M => M.CreatedDate)
                    .FirstOrDefaultAsync();

                //判斷是否已過期
                if (Result != null && DateTime.Now < Result.VerifyCodeExpiryDate)
                {
                    return false; // 驗證碼未過期
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetForgotPassword Error : {ex.Message}");
            }

            return true;
        }

        /// <summary>
        /// 新增忘記密碼表單
        /// </summary>
        /// <param name="Member"></param>
        /// <returns></returns>
        public async Task<string> CreateForgotPassword(Members Member)
        {
            try
            {
                //取隨機數 (5位數補0)
                var RanVerifyCode = new Random().Next(99999).ToString("D5");

                var Form = new ForgotPassword()
                {
                    MemberID = Member.MemberID,// 設置外鍵
                    Member = Member,// 設置導航屬性
                    VerifyCode = RanVerifyCode,
                    VerifyCodeExpiryDate = DateTime.Now.AddMinutes(5),//驗證碼到期時限為5分鐘後
                    CreatedDate = DateTime.Now
                };

                Context.Add(Form);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetForgotPassword Error : {ex.Message}");
                return "GetForgotPassword Error";
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VerifyCode"></param>
        /// <returns></returns>
        public async Task<bool> IsForgotPasswordExists(string VerifyCode)
        {
            return await Context.ForgotPassword.AnyAsync(V => V.VerifyCode == VerifyCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsForgotPasswordExists(int id)
        {
            return Context.ForgotPassword.Any(e => e.SN == id);
        }
    }
}
