using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.DTOs
{
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// 電子郵件
        /// </summary>
        [EmailAddress]
        [StringLength(50)]
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件")]
        public string? Email { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Display(Name = "驗證碼")]
        [Column(TypeName = "char")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "驗證碼長度必須為5個字元")]
        public string? VerifyCode { get; set; }
    }
}
