using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace AniwalkServer.DTOs
{
    public class PageSelectDTO
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
        /// Action名稱
        /// </summary>
        public string ActionName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> FilterRouteValues { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActionName">Action名稱</param>
        /// <param name="TotalDataCount">資料總數</param>
        /// <param name="CurrentPage">當前頁數</param>
        /// <param name="PageSize">一頁資料內有幾筆資料</param>
        /// <param name="PageCount">總頁數</param>
        public PageSelectDTO(string ActionName, int TotalDataCount, int CurrentPage, int PageSize, int PageCount, IDictionary<string, string> FilterRouteValues)
        {
            this.ActionName = ActionName;
            this.TotalDataCount = TotalDataCount;
            this.CurrentPage = CurrentPage;
            this.PageSize = PageSize;
            this.PageSize = PageCount;
            this.FilterRouteValues = FilterRouteValues;
        }
    }
}
