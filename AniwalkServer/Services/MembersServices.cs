using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        /// 取所有會員
        /// </summary>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Members>> GetMembers(int Skip = 0, int Take = 0)
        {
            var Result = Context.Members
                .Include(C => C.Country)
                .Include(R => R.MemberRole)
                .Include(S => S.MemberStatus).ThenInclude(SC => SC.MemberStatusCode)
                .Skip(Skip);

            if (Take > 0)
                Result = Result.Take(Take);

            return await Result.OrderByDescending(C => C.CreatedDate).ToListAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        public async Task<SelectList> GetMemberStatusCodeSelect(string? StatusCode = null)
        {
            var Result = await Context.MemberStatusCode.OrderBy(C => C.StatusCode).ToListAsync();

            return new SelectList(Result, nameof(MemberStatusCode.StatusCode), nameof(MemberStatusCode.StatusName), StatusCode);
        }

    }
}
