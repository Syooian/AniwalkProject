using AniwalkServer.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace AniwalkServer.DTOs
{
    public class ForgotPasswordDTO
    {
        /// <summary>
        /// 電子郵件
        /// </summary>
        [Display(Name = "電子郵件")]
        [EmailCheck]
        public string? Email { get; set; }

        /// <summary>
        /// 驗證碼
        /// </summary>
        [Display(Name = "驗證碼")]
        [Column(TypeName = "char")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "驗證碼長度必須為5個字元")]
        public string? VerifyCode { get; set; }

        /// <summary>
        /// 新的密碼
        /// </summary>
        [DataType(DataType.Password)]
        [PasswordCheck]
        public string? NewPassword { get; set; }
        /// <summary>
        /// 再輸入一次密碼
        /// </summary>
        [DataType(DataType.Password)]
        [PasswordCheck]
        public string? NewPasswordConfirm { get; set; }

        /// <summary>
        /// 驗證階段
        /// </summary>
        public ForgotPasswordDTOPhase Phase { get; set; } = ForgotPasswordDTOPhase.Email;
    }

    /// <summary>
    /// 驗證階段
    /// </summary>
    public enum ForgotPasswordDTOPhase
    {
        /// <summary>
        /// 輸入Email
        /// </summary>
        Email,
        /// <summary>
        /// 輸入驗證碼
        /// </summary>
        VerifyCode,
        /// <summary>
        /// 修改密碼
        /// </summary>
        ChangePassword
    }
}
