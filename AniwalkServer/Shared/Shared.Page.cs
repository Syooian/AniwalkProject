using System.Diagnostics;

namespace AniwalkServer
{
    public partial class Shared
    {
        /// <summary>
        /// 取得資料總頁數
        /// </summary>
        /// <param name="TotalDataCount">資料總筆數</param>
        /// <param name="PageSize">一頁資料內有多少筆資料</param>
        /// <returns></returns>
        public static int GetPageCount(int TotalDataCount, int PageSize)
        {
            if (PageSize == (int)DefaultPageSize.Max)
                return 1;
            else
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

        /// <summary>
        /// 檢查是否顯示頁數選擇
        /// </summary>
        /// <param name="PageCount"></param>
        /// <param name="TotalDataCount"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static bool CheckShowPage(int PageCount, int TotalDataCount, int PageSize)
        {
            //Debug.WriteLine($"CheckShowPage PageCount : {PageCount}, TotalDataCount : {TotalDataCount}, PageSize : {PageSize}");

            if (PageCount > 1 && TotalDataCount > PageSize)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    ///  預設一頁資料內有幾筆資料
    /// </summary>
    public enum DefaultPageSize
    {
        /// <summary>
        /// 全部
        /// </summary>
        Max = 0,
        /// <summary>
        /// 預設一頁資料內有 5 筆資料
        /// </summary>
        PageSize_5 = 5,
        /// <summary>
        /// 預設一頁資料內有 20 筆資料
        /// </summary>
        PageSize_20 = 20,
        /// <summary>
        /// 預設一頁資料內有 40 筆資料
        /// </summary>
        PageSize_40 = 40,
        /// <summary>
        /// 預設一頁資料內有 60 筆資料
        /// </summary>
        PageSize_60 = 60,
        /// <summary>
        /// 預設一頁資料內有 80 筆資料
        /// </summary>
        PageSize_80 = 80
    }
}
