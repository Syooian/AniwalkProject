using System.Diagnostics;

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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public string ToParamString()
        //{
        //    /*
        //     * ASP.NET Core 的 Model Binding 只會自動將查詢字串的每個 key-value（如 ?CountryCode=JPN&AnimeID=1）對應到複雜型別的屬性。
        //        你現在產生的 URL 是：/Visits/ShowVisitsOnList?VisitsParam=CountryCode=JPN&AnimeID=1
        //        這會讓 Model Binder 嘗試找 VisitsParam 這個屬性（但 VisitsParam 是一個物件，不是字串），所以無法自動對應到 VisitsParam 內的屬性。
        //     */

        //    List<string> ParamList = new List<string>();

        //    //如果參數值有特殊字元（如空白、中文），應該用 Uri.EscapeDataString 進行編碼。

        //    if (!string.IsNullOrEmpty(CountryCode))
        //        ParamList.Add($"{nameof(CountryCode)}={Uri.EscapeDataString(CountryCode)}");

        //    if (!string.IsNullOrEmpty(CountryName))
        //        ParamList.Add($"{nameof(CountryName)}={Uri.EscapeDataString(CountryName)}");

        //    if (!string.IsNullOrEmpty(AnimeID))
        //        ParamList.Add($"{nameof(AnimeID)}={Uri.EscapeDataString(AnimeID)}");

        //    if (!string.IsNullOrEmpty(AnimeTitle))
        //        ParamList.Add($"{nameof(AnimeTitle)}={Uri.EscapeDataString(AnimeTitle)}");

        //    if (!string.IsNullOrEmpty(MemberName))
        //        ParamList.Add($"{nameof(MemberName)}={Uri.EscapeDataString(MemberName)}");

        //    if (VisitedDate_From != null && VisitedDate_To != null)
        //        ParamList.Add($"VisitedDate_From={VisitedDate_From:yyyy-MM-dd}&VisitedDate_To={VisitedDate_To:yyyy-MM-dd}");

        //    Debug.WriteLine("ToParamString : " + string.Join("&", ParamList));

        //    return string.Join("&", ParamList);//用 & 串接每個參數
        //}
    }
}
