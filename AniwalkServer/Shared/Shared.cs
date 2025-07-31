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
                    Console.WriteLine($"Key : {key}, Errors : {string.Join(", ", errors.Select(e => e.ErrorMessage))}");
                }
            }
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
