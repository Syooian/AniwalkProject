using Dapper;
using System.Data;
using AniwalkServer.Data;
using AniwalkServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<(int, List<Announcements>)> GetAnnouncements(int Skip = 0, int? Take = 0)
        {
            using (var Connection = Context.Database.GetDbConnection())
            {
                var Result = await Connection.QueryMultipleAsync(
                    "Sp_GetAnnouncements",
                    new { Skip, Take },
                    commandType: CommandType.StoredProcedure
                );

                //總筆數
                var AnnouncementsCount = Result.ReadAsync<int>().Result.First();
                var Announcements = Result.ReadAsync<Announcements>().Result.ToList();

                Debug.WriteLine($"總筆數 : {AnnouncementsCount}, 資料筆數 : {Announcements.Count}");

                return (AnnouncementsCount, Announcements);
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
