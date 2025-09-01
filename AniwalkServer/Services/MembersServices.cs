using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;

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
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<Members> GetMemberByName(string Name)
        {
            return await Context.Members.FirstOrDefaultAsync(M => M.Name == Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<Members> GetMemberByAccount(string Account)
        {
            return await Context.Members
                .Include(L => L.Login)
                .FirstOrDefaultAsync(M => M.Login.Account == Account);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public async Task<MemberStatus> GetMemberStatus(string MemberID)
        {
            var Result = await Context.MemberStatus
                .Include(SC => SC.MemberStatusCode)
                .FirstOrDefaultAsync();

            return Result;
        }

        /// <summary>
        /// 新建會員
        /// </summary>
        /// <param name="RegistData"></param>
        /// <returns></returns>
        public async Task<Result> CreateNewMember(RegistDTO RegistData)
        {
            try
            {
                var NewMember = new Members()
                {
                    Name = RegistData.Name,
                    Email = RegistData.Email,
                    CountryCode = RegistData.CountryCode,
                    CreatedDate = DateTime.Now, // 設定創建日期為當前時間
                    RoleID = (int)RoleEnum.Member, // 設定會員角色為一般會員
                    MemberStatus = new MemberStatus()//新增會員狀態
                };

                //生成MemberID並檢查是否重複
                while (true)
                {
                    var NewMemberID = new Random().Next(0, 999999999).ToString("D10"); // 生成隨機的10位數會員ID
                    if (!Context.Members.Any(m => m.MemberID == NewMemberID)) // 檢查是否已存在相同的會員ID
                    {
                        NewMember.MemberID = NewMemberID; // 如果不存在，則使用這個ID
                        break;
                    }
                }

                //新增帳密
                var Login = new Login()
                {
                    MemberID = NewMember.MemberID, // 設定Login的MemberID為新生成的會員ID
                    Account = RegistData.Account,
                    Password = RegistData.Password
                };

                //將Login資料與Members關聯
                NewMember.Login = Login;

                Context.Add(NewMember);
                await Context.SaveChangesAsync();

                return new Result();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreateNewMember ex : " + ex.Message);

                return new Result(ResultType.Fail, "註冊失敗");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StatusCode"></param>
        /// <returns></returns>
        public async Task<SelectList> GetMemberStatusCodeSelect(int? StatusCode = null)
        {
            var Result = await Context.MemberStatusCode.OrderBy(C => C.StatusCode).ToListAsync();

            return new SelectList(Result, nameof(MemberStatusCode.StatusCode), nameof(MemberStatusCode.StatusName), StatusCode);
        }

    }
}
