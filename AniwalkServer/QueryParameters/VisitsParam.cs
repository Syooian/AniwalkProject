namespace AniwalkServer.QueryParameters
{
    public class VisitsParam
    {
        public int? VisitSN;
        public string CountryName { get; set; }
        public string AnimeTitle { get; set; }
        public string MemberName { get; set; }
        public string VisitedDate { get; set; }
        /// <summary>
        /// 對到訪紀錄照片做排序
        /// </summary>
        public bool SortVisitsPhotos;

        /*
            VisitsParam 屬性是欄位（field），不是屬性（property）。
            ASP.NET Core Model Binding 只會綁定 public property，不會綁定 public field。
            所以需改成{ get; set; }
         */

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"CountryName: {CountryName}, AnimeTitle: {AnimeTitle}, MemberName: {MemberName}, VisitedDate: {VisitedDate}, SortVisitsPhotos: {SortVisitsPhotos}";
        }
    }
}
