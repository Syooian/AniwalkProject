using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace AniwalkServer.Services
{
    public class VisitsServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public VisitsServices(AniwalkDBContext Context) : base(Context) { }

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
        /// 取到訪記錄
        /// <para>同時取資料和總筆數 (多結果集)</para>
        /// </summary>
        /// <param name="VisitsParam"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<PageDTO<VisitsDTO, VisitsParam>> GetVisits(VisitsParam? VisitsParam, int Page = 1, int PageSize = 0)
        {
            if (VisitsParam == null)
                VisitsParam = new VisitsParam();

            Debug.WriteLine($"GetVisits Param : {VisitsParam}, Page : {Page}, PageSize : {PageSize}");

            //直接在資料庫select是完整有東西的，但畫面上卻看不到"到訪國家", "動畫名稱", "會員名稱"
            var SQLQuery = "select V.SN, V.Latitude, V.Longitude, V.MemberID, C.CountryCode, C.CountryName, A.AnimeID, A.Title, V.MainText, M.Name, V.VisitedDate, V.CreatedDate from Visits as V " +
                "join Members as M on V.MemberID = M.MemberID " +
                "join Animes as A on V.AnimeID = A.AnimeID " +
                "join Countries as C on V.CountryCode = C.CountryCode " +
                "where 1=1 ";
            var SQLPara = new DynamicParameters();

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
            if (!string.IsNullOrEmpty(VisitsParam.CountryCode))
            {
                Debug.WriteLine("CountryCode : " + VisitsParam.CountryCode);

                SQLQuery += $"and C.CountryCode = @CountryCode ";
                SQLPara.Add("@CountryCode", VisitsParam.CountryCode);
            }

            if (!string.IsNullOrEmpty(VisitsParam.CountryName))
            {
                Debug.WriteLine("CountryName : " + VisitsParam.CountryName);

                SQLQuery += $"and C.CountryName = @CountryName ";
                SQLPara.Add("@CountryName", VisitsParam.CountryName);
            }

            if (!string.IsNullOrEmpty(VisitsParam.AnimeID))
            {
                Debug.WriteLine("AnimeID : " + VisitsParam.AnimeID);

                SQLQuery += $"and A.AnimeID = @AnimeID ";
                SQLPara.Add("@AnimeID", VisitsParam.AnimeID);
            }

            if (!string.IsNullOrEmpty(VisitsParam.AnimeTitle))
            {
                Debug.WriteLine("AnimeTitle : " + VisitsParam.AnimeTitle);

                SQLQuery += $"and A.Title = @AnimeTitle ";
                SQLPara.Add("@AnimeTitle", VisitsParam.AnimeTitle);
            }

            if (!string.IsNullOrEmpty(VisitsParam.MemberName))
            {
                Debug.WriteLine("MemberName : " + VisitsParam.MemberName);

                SQLQuery += $"and M.Name = @MemberName ";
                SQLPara.Add("@MemberName", VisitsParam.MemberName);
            }

            if (VisitsParam.VisitedDate_From != null && VisitsParam.VisitedDate_To != null)
            {
                Debug.WriteLine($"VisitedDate_From : {VisitsParam.VisitedDate_From}, VisitedDate_To : {VisitsParam.VisitedDate_To}");

                //SQLQuery += $"and V.VisitedDate between CONVERT(varchar, @VisitedDate_From, 111) and CONVERT(varchar, @VisitedDate_To, 111) ";//用資料庫語法轉換日期格式為yyyy/MM/dd
                //VisitedDate_From 和 VisitedDate_To 目前是 DateTime 型別，CONVERT(varchar, @VisitedDate_From, 111) 會把參數轉成字串（如 2025/07/29），但 V.VisitedDate 是 datetime 型別，這樣比較會失敗或無法正確查詢。
                //直接用 datetime 型別做比較，不需要 CONVERT
                //只比對日期，忽略時間
                SQLQuery += $"and convert(date, V.VisitedDate) between @VisitedDate_From and @VisitedDate_To ";

                SQLPara.Add("@VisitedDate_From", VisitsParam.VisitedDate_From);
                SQLPara.Add("@VisitedDate_To", VisitsParam.VisitedDate_To);
            }
            #endregion

            //加入排序
            SQLQuery += "order by V.CreatedDate desc ";

            #region 加入分頁查詢參數
            if (PageSize == 0)//帶入0表示選擇全部不篩選
            {
                SQLQuery += ";";//補一個結束符號
            }
            else
            {
                if (Page < 1)//防呆
                    Page = 1;

                SQLPara.Add("@Skip", Shared.GetSkip(Page, PageSize));
                SQLPara.Add("@Take", PageSize);

                Debug.WriteLine($"Skip {Shared.GetSkip(Page, PageSize)}, Take : {PageSize}");

                SQLQuery += "offset @Skip rows fetch next @Take rows only; ";
            }
            #endregion

            //依建立日期排序
            //SQLQuery += "order by V.CreatedDate desc;";

            #region 與查資料總數語句合併
            var SQL = "select count(*) as 'TotalDataCount' from Visits;" + SQLQuery;
            #endregion

            Debug.WriteLine("SQL : " + SQL);

            var Connection = Context.Database.GetDbConnection();

            try
            {
                //手動開啟連線
                //if (Connection.State != ConnectionState.Open)
                //{
                //    Debug.WriteLine("手動開啟連線");
                //    await Connection.OpenAsync();
                //}

                var Result = await Connection.QueryMultipleAsync(SQL, SQLPara, commandType: CommandType.Text);

                //接收資料
                var Data = new PageDTO<VisitsDTO, VisitsParam>(
                    Result,
                    Page,//當前頁碼
                    PageSize,//每頁筆數
                    VisitsParam//篩選參數
                );

                return Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetVisits Error : {ex.Message}");

                return null;
            }
            finally
            {
                Connection.Close();
            }

            //雖然直接加入include能夠正常顯示"到訪國家", "動畫名稱", "會員名稱"，但因為已經在SQLQuery join了Members, Animes, Countries，因此再加include等於做了重複的事，浪費資源
            //var VisitsDTO = await Context.VisitsDTO.FromSqlRaw(SQLQuery, SQLPara.ToArray())
            //    //.Include(V => V.Member)
            //    //.Include(V => V.Anime)
            //    //.Include(V => V.Country)
            //    .OrderByDescending(V => V.CreatedDate)
            //    .ToListAsync();

            //將VisitsDTO轉換成Visits
            //var Visits = new List<Visits>();
            //foreach (var Visit in VisitsDTO)
            //{
            //    //Debug.WriteLine(Visit.CountryCode);
            //    //Debug.WriteLine(Visit.CountryName);

            //    Visits.Add(new Visits
            //    {
            //        SN = Visit.SN,
            //        MainText = Visit.MainText,
            //        Latitude = Visit.Latitude,
            //        Longitude = Visit.Longitude,
            //        VisitedDate = Visit.VisitedDate,
            //        CreatedDate = Visit.CreatedDate,
            //        MemberID = Visit.MemberID,
            //        Member = new Members { MemberID = Visit.MemberID, Name = Visit.Name },
            //        CountryCode = Visit.CountryCode,
            //        Country = new Countries { CountryCode = Visit.CountryCode, CountryName = Visit.CountryName },
            //        AnimeID = Visit.AnimeID,
            //        Anime = new Animes { AnimeID = Visit.AnimeID, Title = Visit.Title }
            //    });
            //}

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

            //return Visits;
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
