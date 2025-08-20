namespace AniwalkServer.QueryParameters
{
    public class VisitsParam
    {
        public int? VisitSN;
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; }
        public string AnimeID { get; set; } = null!;
        public string AnimeTitle { get; set; }
        public string MemberName { get; set; } = null!;
        public DateTime? VisitedDate_From { get; set; }
        public DateTime? VisitedDate_To { get; set; }
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
            var Msg =
                $"CountryCode : {(string.IsNullOrEmpty(CountryCode) ? null : CountryCode)}, " +
                $"CountryName: {(string.IsNullOrEmpty(CountryName) ? null : CountryName)}, " +
                $"AnimeID : {(string.IsNullOrEmpty(AnimeID) ? null : AnimeID)}, " +
                $"AnimeTitle : {(string.IsNullOrEmpty(AnimeTitle) ? null : AnimeTitle)}, " +
                $"MemberName : {(string.IsNullOrEmpty(MemberName) ? null : MemberName)}, " +
                "VisitedDate : ";

            if (VisitedDate_From != null && VisitedDate_To != null)
                Msg += $"From {VisitedDate_From} To {VisitedDate_To}";
            else
                Msg += "不可用";

            return Msg;
        }
    }
}
