using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

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
