using AniwalkServer.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AniwalkServer
{
    public partial class Shared
    {
        /// <summary>
        /// 訪客角色名稱
        /// </summary>
        public const string Role_Guest = "Guest";
        /// <summary>
        /// 一般會員角色名稱
        /// </summary>
        public const string Role_Member = "Member";
        /// <summary>
        /// 管理員角色名稱
        /// </summary>
        public const string Role_Admin = "Admin";

        /// <summary>
        /// 身份驗證方案名稱
        /// </summary>
        public const string AuthenticationScheme = "UserLogin"; // 定義一個常數用於身份驗證方案名稱

        /// <summary>
        /// 到訪照片根路徑
        /// </summary>
        public const string VisitsPhotosRootPath = "VisitsPhotos";
        /// <summary>
        /// 取到訪紀錄照片路徑
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="PhotoID"></param>
        /// <param name="PhotoType"></param>
        /// <returns></returns>
        public static string GetVisitsPhotoPath(string MemberID, string PhotoID, string PhotoType)
        {
            return $"{VisitsPhotosRootPath}/{MemberID}/{PhotoID}{PhotoType}";
        }

        /// <summary>
        /// 
        /// </summary>
        public const string AnimesPhotosRootPath = "AnimesPhotos";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnimeID"></param>
        /// <param name="PhotoName"></param>
        /// <returns></returns>
        public static string GetAnimesPhotosPath(string AnimeID, string PhotoName)
        {
            return $"{AnimesPhotosRootPath}/{AnimeID}/{PhotoName}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ModelState"></param>
        public static void ShowModelState(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState)
        {
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                if (errors.Any())
                {
                    Debug.WriteLine($"Key : {key}, Errors : {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
                }
            }
        }

        /// <summary>
        /// 截斷文字
        /// </summary>
        /// <param name="Msg">文字內容</param>
        /// <param name="LimitLength">限制長度</param>
        /// <returns></returns>
        public static string TruncateText(string Msg, int LimitLength)
        {
            if (!string.IsNullOrEmpty(Msg))
            {
                if (Msg.Length > LimitLength)
                    return Msg.Substring(0, LimitLength) + "......";
                else
                    return Msg;
            }

            return "";
        }

        /// <summary>
        /// 轉換斷行符號
        /// <para>使用時須用Html.Raw包起來才會正常顯示斷行的效果</para>
        /// </summary>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static string ConvertNewLineToBr(string Msg)
        {
            if (!string.IsNullOrEmpty(Msg))
            {
                return Msg.Replace("\r\n", "<br>");
            }

            return "";
        }

        /// <summary>
        /// 取得 MapData 並進行 URI 編碼
        /// </summary>
        /// <param name="ViewData"></param>
        /// <returns></returns>
        public static string GetMapDataJsonEscapeDataString(ViewDataDictionary ViewData)
        {
            if (ViewData.TryGetValue(ViewDataKeys.MapData, out object Data))
                if (Data != null)
                    return Uri.EscapeDataString(Data.ToString());

            return "";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RoleEnum
    {
        /// <summary>
        /// 訪客
        /// </summary>
        Guest = 0,
        /// <summary>
        /// 一般會員
        /// </summary>
        Member = 1,
        /// <summary>
        /// 管理員
        /// </summary>
        Admin = 9
    }
}
