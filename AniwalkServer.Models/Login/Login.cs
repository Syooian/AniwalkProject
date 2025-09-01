using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 帳密
    /// </summary>
    public class Login
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Display(Name = "帳號")]
        [Key]
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "帳號為5~20碼")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "帳號只能包含英文字母和數字")]
        public string Account { get; set; } = null!;
        /// <summary>
        /// 密碼
        /// </summary>
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "密碼為5~20碼")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "密碼只能包含英文字母和數字")]
        public string Password { get; set; } = null!;

        #region 外鍵關聯
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Member))]
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Members? Member { get; set; } = null!;
        #endregion
    }
}
