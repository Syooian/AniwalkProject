using System.Diagnostics;

namespace AniwalkServer
{
    public partial class Shared
    {
        /// <summary>
        /// 預設一頁資料內有多少筆資料
        /// </summary>
        public static readonly int[] DefaultPageSize = { 20, 40, 60, 80 };
        /// <summary>
        /// 取得資料總頁數
        /// </summary>
        /// <param name="TotalDataCount">資料總筆數</param>
        /// <param name="PageSize">一頁資料內有多少筆資料</param>
        /// <returns></returns>
        public static int GetPageCount(int TotalDataCount, int PageSize)
        {
            return (TotalDataCount + PageSize - 1) / PageSize;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static int GetSkip(int Page, int PageSize)
        {
            return (Page - 1) * PageSize;
        }
    }
}
