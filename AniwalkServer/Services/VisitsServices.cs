using AniwalkServer.Data;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace AniwalkServer.Services
{
    public class VisitsServices
    {
        /// <summary>
        /// 
        /// </summary>
        readonly AniwalkDBContext Context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public VisitsServices(AniwalkDBContext Context)
        {
            this.Context = Context;
        }

        /// <summary>
        /// 到訪紀錄是否存在
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <returns></returns>
        private bool IsVisitExists(int VisitSN)
        {
            return Context.Visits.Any(V => V.SN == VisitSN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitsParam"></param>
        /// <returns></returns>
        public async Task<List<Visits>> GetVisits(VisitsParam? VisitsParam)
        {
            if (VisitsParam == null)
            {
                VisitsParam = new VisitsParam();
            }

            return await GetVisits(VisitsParam.CountryName, VisitsParam.AnimeTitle, VisitsParam.MemberName, VisitsParam.VisitedDate, VisitsParam.SortVisitsPhotos);
        }
        /// <summary>
        /// 對到訪紀錄照片做排序
        /// </summary>
        /// <param name="CountryName"></param>
        /// <param name="AnimeTitle"></param>
        /// <param name="MemberName"></param>
        /// <param name="VisitedDate"></param>
        /// <param name="SortVisitsPhotos"></param>
        /// <returns></returns>
        public async Task<List<Visits>> GetVisits(string? CountryName, string? AnimeTitle, string? MemberName, string? VisitedDate, bool SortVisitsPhotos = false)
        {
            //直接在資料庫select是完整有東西的，但畫面上卻看不到"到訪國家", "動畫名稱", "會員名稱"
            var SQLQuery = "select V.SN, V.Latitude, V.Longitude, V.MemberID, C.CountryCode, C.CountryName, A.AnimeID, A.Title, V.MainText, M.Name, V.VisitedDate, V.CreatedDate from Visits as V " +
                "join Members as M on V.MemberID = M.MemberID " +
                "join Animes as A on V.AnimeID = A.AnimeID " +
                "join Countries as C on V.CountryCode = C.CountryCode " +
                "where 1=1 ";
            var SQLPara = new List<SqlParameter>();

            //SQLQuery A
            //var SQLQuery = "select * from Visits as V where 1=1 ";

            //var SQLQuery = $"select V.*, M.*, A.*, C.*, VP.* from Visits as V " +
            //    $"join Members as M on V.MemberID = M.MemberID " +
            //    $"join Animes as A on V.AnimeID = A.AnimeID " +
            //    $"join Countries as C on V.CountryCode = C.CountryCode " +
            //    $"join VisitsPhotos as VP on V.SN = VP.SN " +
            //    $"where 1=1 ";

            /*
             select V.SN, C.CountryName, A.Title, V.MainText, M.Name, V.VisitedDate from Visits as V
                join Members as M on V.MemberID = M.MemberID
                join Animes as A on V.AnimeID = A.AnimeID
                join Countries as C on V.CountryCode = C.CountryCode
                where 1=1
             */

            /*
             select V.*, M.*, A.*, C.*, VP.* from Visits as V
	            join Members as M on V.MemberID = M.MemberID
	            join Animes as A on V.AnimeID = A.AnimeID
	            join Countries as C on V.CountryCode = C.CountryCode
	            join VisitsPhotos as VP on V.SN = VP.SN
	            where 1=1
	            order by V.CreatedDate desc;
             */

            #region 篩選條件
            if (!string.IsNullOrEmpty(CountryName))
            {
                SQLQuery += $"and C.CountryName = @CountryName ";
                SQLPara.Add(new SqlParameter("@CountryName", CountryName));
            }

            if (!string.IsNullOrEmpty(AnimeTitle))
            {
                SQLQuery += $"and A.Title = @AnimeTitle ";
                SQLPara.Add(new SqlParameter("@AnimeTitle", AnimeTitle));
            }

            if (!string.IsNullOrEmpty(MemberName))
            {
                SQLQuery += $"and M.Name = @MemberName ";
                SQLPara.Add(new SqlParameter("@MemberName", MemberName));
            }

            if (!string.IsNullOrEmpty(VisitedDate))
            {
                //SQLQuery += $"and V.VisitedDate = @VisitedDate ";
                SQLQuery += $"and CONVERT(varchar, V.VisitedDate, 111) = @VisitedDate ";
                SQLPara.Add(new SqlParameter("@VisitedDate", VisitedDate));
            }
            #endregion

            //依建立日期排序
            //SQLQuery += "order by V.CreatedDate desc;";

            //雖然直接加入include能夠正常顯示"到訪國家", "動畫名稱", "會員名稱"，但因為已經在SQLQuery join了Members, Animes, Countries，因此再加include等於做了重複的事，浪費資源
            var VisitsDTO = await Context.VisitsDTO.FromSqlRaw(SQLQuery, SQLPara.ToArray())
                //.Include(V => V.Member)
                //.Include(V => V.Anime)
                //.Include(V => V.Country)
                .OrderByDescending(V => V.CreatedDate)
                .ToListAsync();

            //將VisitsDTO轉換成Visits
            var Visits = new List<Visits>();
            foreach (var Visit in VisitsDTO)
            {
                //Debug.WriteLine(Visit.CountryCode);
                //Debug.WriteLine(Visit.CountryName);

                Visits.Add(new Visits
                {
                    SN = Visit.SN,
                    MainText = Visit.MainText,
                    Latitude = Visit.Latitude,
                    Longitude = Visit.Longitude,
                    VisitedDate = Visit.VisitedDate,
                    CreatedDate = Visit.CreatedDate,
                    MemberID = Visit.MemberID,
                    Member = new Members { MemberID = Visit.MemberID, Name = Visit.Name },
                    CountryCode = Visit.CountryCode,
                    Country = new Countries { CountryCode = Visit.CountryCode, CountryName = Visit.CountryName },
                    AnimeID = Visit.AnimeID,
                    Anime = new Animes { AnimeID = Visit.AnimeID, Title = Visit.Title }
                });
            }

            //SQLQuery A搭配這個有效，但無法自訂其他搜索條件
            //var Visits = await Context.Visits.FromSqlRaw(SQLQuery)
            //    .Include(V => V.Member)
            //    .Include(V => V.Anime)
            //    .Include(V => V.Country)
            //    .ToListAsync();

            //var Visits = await Context.Visits
            //    .Include(V => V.Member)
            //    .Include(V => V.Anime)
            //    .Include(V => V.Country)
            //    .Include(V => V.VisitsPhotos)
            //    .OrderByDescending(V => V.CreatedDate)
            //    .ToListAsync();

            //if (SortVisitsPhotos)
            //{
            //    foreach (var Visit in Visits)
            //    {
            //        Visit.VisitsPhotos = SortVisitPhotos(Visit.VisitsPhotos);
            //    }
            //}

            return Visits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VisitSN"></param>
        /// <param name="SortVisitsPhotos">對到訪紀錄照片做排序</param>
        /// <returns></returns>
        public async Task<Visits> GetVisit(int VisitSN, bool SortVisitsPhotos = false)
        {
            var Visit = await Context.Visits
                .Include(V => V.Member)
                .Include(V => V.Anime)
                .Include(V => V.Country)
                .Include(V => V.VisitsPhotos)
                .FirstOrDefaultAsync(V => V.SN == VisitSN);

            if (SortVisitsPhotos)
            {
                Visit.VisitsPhotos = SortVisitPhotos(Visit.VisitsPhotos);
            }

            return Visit;
        }

        /// <summary>
        /// 排序到訪紀錄照片
        /// </summary>
        /// <param name="VisitPhotos"></param>
        List<VisitsPhotos>? SortVisitPhotos(List<VisitsPhotos>? VisitPhotos)
        {
            if (VisitPhotos == null)
                return null;

            return VisitPhotos.OrderBy(N => N.SortNumber).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Visit"></param>
        /// <returns></returns>
        public async Task<Visits> UpdateVisit(Visits Visit)
        {
            Context.Entry(Visit).State = EntityState.Modified;

            try
            {
                //Context.Update(Visit);

                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Visit;
        }
    }
}
