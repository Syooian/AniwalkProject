using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Services
{
    public class MembersServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public MembersServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

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
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<Members?> GetMember(string MemberID)
        {
            var Member = await Context.Members
                       .Include(M => M.Country)
                       .FirstOrDefaultAsync(M => M.MemberID == MemberID);

            return Member;
        }
    }
}
