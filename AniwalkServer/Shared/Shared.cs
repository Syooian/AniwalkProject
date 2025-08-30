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
        /// 到訪照片根路徑
        /// </summary>
        public const string VisitsPhotosRootPath = "VisitsPhotos";

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
        /// 
        /// </summary>
        /// <param name="ViewData"></param>
        /// <returns></returns>
        public static string GetMapDataJsonString(ViewDataDictionary ViewData)
        {
            if (ViewData.TryGetValue(ViewDataKeys.MapData, out object Data))
                return JsonConvert.SerializeObject(Data);
            else
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
