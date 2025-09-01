using Dapper;
using System.Diagnostics;
using System.Reflection;

namespace AniwalkServer.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="DataT"></typeparam>
    public class PageDTO<DataT> : PageDTO<DataT, object>
    {
        /*
         * C# 的泛型設計中，泛型類別的型別參數（如 PageDTO<DataT, FilterT>）無法直接指定預設型別，所以另外建立一個只帶一個型別參數的類別，讓 FilterT 預設為 object 或 null。
         */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TotalDataCount"></param>
        /// <param name="Data"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        public PageDTO(int TotalDataCount, List<DataT> Data, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        : base(TotalDataCount, Data, CurrentPage, PageSize) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        public PageDTO(SqlMapper.GridReader Result, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20)
        : base(Result, CurrentPage, PageSize) { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="DataT">資料</typeparam>
    /// <typeparam name="FilterT">篩選條件</typeparam>
    public class PageDTO<DataT, FilterT> where FilterT : class
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
        public List<DataT> Data { get; private set; }
        /// <summary>
        /// 篩選資料
        /// </summary>
        public FilterT Filter { get; private set; }

        /// <summary>
        /// 設置總頁數
        /// </summary>
        /// <param name="TotalDataCount"></param>
        /// <param name="Data"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="Filter"></param>
        public PageDTO(int TotalDataCount, List<DataT> Data, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20, FilterT Filter = null)
        {
            this.TotalDataCount = TotalDataCount;
            this.Data = Data;
            this.CurrentPage = CurrentPage;
            PageCount = Shared.GetPageCount(TotalDataCount, PageSize);
            this.Filter = Filter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="Filter"></param>
        public PageDTO(SqlMapper.GridReader Result, int CurrentPage = 1, int PageSize = (int)DefaultPageSize.PageSize_20, FilterT Filter = null)
        {
            TotalDataCount = Result.Read<int>().First();
            Data = Result.Read<DataT>().ToList();
            this.CurrentPage = CurrentPage;
            PageCount = Shared.GetPageCount(TotalDataCount, PageSize);
            this.Filter = Filter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetFilterRouteValues()
        {
            var FilterDic = new Dictionary<string, string>();
            if (Filter != null)
            {
                foreach (var Item in Filter.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var Value = Item.GetValue(Filter);
                    if (Value != null)
                    {
                        Debug.WriteLine($"Add To FilterDic {Item.Name}:{Value}");

                        FilterDic[Item.Name] = Value.ToString();
                    }
                }
            }

            return FilterDic;
        }
    }
}
