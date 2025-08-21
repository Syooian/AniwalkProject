using Dapper;
using System.Data;
using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using AniwalkServer.DTOs;

namespace AniwalkServer.Services
{
    public class AnnouncementsServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public AnnouncementsServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 取公告
        /// <para>同時取資料和總筆數 (多結果集)</para>
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<PageDTO<Announcements>> GetAnnouncements(int Page = 1, int PageSize = 0)
        {
            using (var Connection = Context.Database.GetDbConnection())
            {
                //Skip : 跳過開頭幾筆紀錄
                //Take : 取幾筆紀錄

                Debug.WriteLine($"GetAnnouncements Page : {Page}, PageSize : {PageSize}");

                try
                {
                    var Result = await Connection.QueryMultipleAsync(
                        "Sp_GetAnnouncements",
                        new { Skip = Shared.GetSkip(Page, PageSize), Take = PageSize },
                        commandType: CommandType.StoredProcedure
                    );

                    var DTO = new PageDTO<Announcements>();
                    //頁碼
                    DTO.CurrentPage = Page;
                    //資料總筆數
                    DTO.TotalDataCount = Result.ReadAsync<int>().Result.First();
                    //資料
                    DTO.Data = Result.ReadAsync<Announcements>().Result.ToList();

                    Debug.WriteLine($"頁碼 : {Page}, 總筆數 : {DTO.TotalDataCount}, 取得資料筆數 : {DTO.Data.Count}");

                    return DTO;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetAnnouncements Error : {ex.Message}");

                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<Announcements> GetAnnouncement(int SN)
        {
            return await Context.Announcements.FindAsync(SN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAnnouncementsExists(int id)
        {
            return Context.Announcements.Any(e => e.SN == id);
        }
    }
}
