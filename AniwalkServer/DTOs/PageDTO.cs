namespace AniwalkServer.DTOs
{
    public class PageDTO<T>
    {
        /// <summary>
        /// 資料總數
        /// </summary>
        public int TotalDataCount { get; set; }
        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 資料
        /// </summary>
        public List<T> Data { get; set; }
    }
}
