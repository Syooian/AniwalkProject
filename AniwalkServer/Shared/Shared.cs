using System.Diagnostics;

namespace AniwalkServer
{
    public class Shared
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
        /// 取得資料總頁數
        /// </summary>
        /// <param name="DataCount">資料總筆數</param>
        /// <param name="PageSize">一頁資料內有多少筆資料</param>
        /// <returns></returns>
        public static int GetPageCount(int DataCount, int PageSize)
        {
            return (DataCount + PageSize - 1) / PageSize;
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
