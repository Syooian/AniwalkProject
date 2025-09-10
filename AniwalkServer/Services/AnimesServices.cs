using AniwalkServer.Data;
using AniwalkServer.DTOs;
using AniwalkServer.Models;
using AniwalkServer.QueryParameters;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AniwalkServer.Services
{
    public class AnimesServices : ServicesBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public AnimesServices(AniwalkDBContext Context) : base(Context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimesParam"></param>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<PageDTO<Animes, AnimesParam>> GetAnimes(AnimesParam? AnimesParam, int Page = 1, int PageSize = 0)
        {
            //總數查詢
            var SQLCount = "select count(*) ";

            //資料查詢
            var SQLData = "Select A.AnimeID, A.Title, A.CreatedDate, A.DisabledDate from Animes as A ";

            //查詢條件
            var SQLSelect = "where 1=1 ";
            //查詢條件參數
            var SQLPara = new DynamicParameters();

            #region 篩選條件
            if (AnimesParam == null)
                AnimesParam = new AnimesParam();

            if (!string.IsNullOrEmpty(AnimesParam.AnimeTitle))
            {
                SQLSelect += $"and A.Title = @AnimeTitle ";
                SQLPara.Add("@AnimeTitle", AnimesParam.AnimeTitle);
            }
            #endregion

            //將資料查詢加入查詢條件和資料排序 (order by 必須在Skip和Take之前)
            SQLData += SQLSelect + "order by A.Title asc ";

            #region 加入數量查詢和分頁查詢參數
            SQLCount += "from Animes ";

            if (PageSize == 0)
            {
                SQLData += ";";//補一個結束符號

                //查詢資料總數
                SQLCount += ";";
            }
            else
            {
                if (Page < 1)//防呆
                    Page = 1;

                SQLPara.Add("@Skip", Shared.GetSkip(Page, PageSize));
                SQLPara.Add("@Take", PageSize);

                SQLData += "offset @Skip rows fetch next @Take rows only;";

                //查詢資料總數 (加入查詢條件)
                SQLCount += "as A " + SQLSelect;
            }
            #endregion

            #region 對資料庫下查詢
            try
            {
                var Connection = Context.Database.GetDbConnection();

                var Result = await Connection.QueryMultipleAsync(SQLCount + SQLData, SQLPara, commandType: System.Data.CommandType.Text);

                //接收資料
                var Data = new PageDTO<Animes, AnimesParam>(
                    Result,
                    Page,//當前頁碼
                    PageSize,//每筆頁數
                    AnimesParam//篩選參數
                );

                return Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetAnimes Error : {ex.Message}");

                return null;
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <returns></returns>
        public async Task<Animes> GetAnime(string AnimeID)
        {
            return await Context.Animes.FindAsync(AnimeID);
        }

        /// <summary>
        /// 產生一個新的動畫ID
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNewAnimeID()
        {
            var Result = await Context.Animes.CountAsync();

            return (Result + 1).ToString("0000");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <param name="IncludeDisabled">是否包含已停用的動畫</param>
        /// <returns></returns>
        public async Task<SelectList> GetAnimeTitlesSelect(string? AnimeID = null, bool IncludeDisabled = false)
        {
            var Query = Context.Animes.AsQueryable();

            //是否包含已停用的動畫
            if (!IncludeDisabled)
                Query = Query.Where(A => A.DisabledDate == null);

            var Result = await Query.OrderBy(A => A.Title).ToListAsync();

            return new SelectList(Result, nameof(Animes.AnimeID), nameof(Animes.Title), AnimeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <param name="IncludeDisabled"></param>
        /// <returns></returns>
        public async Task<SelectList> GetAnimeIDsSelect(string? AnimeID = null, bool IncludeDisabled = false)
        {
            var Query = Context.Animes.AsQueryable();

            //是否包含已停用的動畫
            if (!IncludeDisabled)
                Query = Query.Where(A => A.DisabledDate == null);

            var Result = await Query.OrderBy(A => A.AnimeID).ToListAsync();

            return new SelectList(Result, nameof(Animes.AnimeID), nameof(Animes.AnimeID), AnimeID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <returns></returns>
        public async Task<bool> IsAnimeExists(string AnimeID)
        {
            return await Context.Animes.AnyAsync(e => e.AnimeID == AnimeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public async Task<bool> IsAnimeExistsByTitle(string Title)
        {
            return await Context.Animes.AnyAsync(A => A.Title.Contains(Title));
        }
    }
}
