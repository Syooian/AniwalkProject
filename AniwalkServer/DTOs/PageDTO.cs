using Dapper;

namespace AniwalkServer.DTOs
{
    public class PageDTO<T>
    {
        /// <summary>
        /// 資料總數
        /// </summary>
        public int TotalDataCount { get; private set; }
        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; private set; }
        /// <summary>
        /// 一頁資料內有幾筆資料
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 總頁數
        /// </summary>
        public int PageCount { get; private set; }
        /// <summary>
        /// 資料
        /// </summary>
        public List<T> Data { get; private set; }

        /// <summary>
        /// 設置總頁數
        /// </summary>
        /// <param name="TotalDataCount"></param>
        /// <param name="Data"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        public PageDTO(int TotalDataCount, List<T> Data, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        {
            this.TotalDataCount = TotalDataCount;
            this.Data = Data;
            this.CurrentPage = CurrentPage;
            PageCount = Shared.GetPageCount(TotalDataCount, PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        public PageDTO(SqlMapper.GridReader Result, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        {
            TotalDataCount = Result.Read<int>().First();
            Data = Result.Read<T>().ToList();
            this.CurrentPage = CurrentPage;
            PageCount = Shared.GetPageCount(TotalDataCount, PageSize);
        }
    }
}
