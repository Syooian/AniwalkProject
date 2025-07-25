namespace AniwalkServer
{
    public class Shared
    {
        /// <summary>
        /// 一般會員角色名稱
        /// </summary>
        public const string Role_Member = "Member";
        /// <summary>
        /// 管理員角色名稱
        /// </summary>
        public const string Role_Admin = "Admin";
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
