using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class MembersServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public MembersServices(AniwalkDBContext Context) : base(Context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public bool IsMembersExists(string MemberID)
        {
            return Context.Members.Any(M => M.MemberID == MemberID);
        }

        /// <summary>
        /// 由MemberID查找使用者
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<Members?> GetMember(string MemberID)
        {
            var Member = await Context.Members
                .Include(M => M.Country)
                .Include(S => S.MemberStatus).ThenInclude(SC => SC.MemberStatusCode)
                .FirstOrDefaultAsync(M => M.MemberID == MemberID);

            return Member;
        }

        /// <summary>
        /// 由Email查找使用者
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        public async Task<Members> GetMemberByEmail(string Email)
        {
            return await Context.Members.FirstOrDefaultAsync(M => M.Email == Email);
        }
    }
}
