namespace AniwalkServer.QueryParameters
{
    public class VisitsParam
    {
        public int? VisitSN;
        public bool IncludeMember;
        public bool IncludeAnime;
        public bool IncludeCountry;
        public bool IncludeVisitsPhotos;
        /// <summary>
        /// 對到訪紀錄照片做排序
        /// </summary>
        public bool SortVisitsPhotos;
    }
}
