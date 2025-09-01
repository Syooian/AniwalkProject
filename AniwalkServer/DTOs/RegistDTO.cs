using AniwalkServer.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace AniwalkServer.DTOs
{
    public class RegistDTO : ValidationAttribute
    {
        /// <summary>
        /// 會員名稱
        /// </summary>
        [StringLength(40)]
        [Display(Name = "會員名稱")]
        [Required(ErrorMessage = "請輸入會員名稱")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// 電子郵件
        /// <para>ForgotPasswordDTO也有一個Email，如果將來要修改驗證規則須連那個一起改</para>
        /// </summary>
        [EmailAddress]
        [StringLength(50)]
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "請輸入電子郵件")]
        public string Email { get; set; } = null!;
        /// <summary>
        /// 國家代碼
        /// </summary>
        [HiddenInput]
        [Display(Name = "所在國家")]
        [Required(ErrorMessage = "請選擇所在國家")]
        public string CountryCode { get; set; } = null!;
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
        /// 輸入密碼
        /// </summary>
        [Display(Name = "輸入密碼")]
        [DataType(DataType.Password)]
        //[Required(ErrorMessage = "請輸入密碼")]
        [PasswordCheck]
        public string Password { get; set; } = null!;
        /// <summary>
        /// 再次輸入密碼
        /// </summary>
        [Display(Name = "再次輸入密碼")]
        [DataType(DataType.Password)]
        //[Required(ErrorMessage = "請輸入密碼")]
        [PasswordCheck]
        public string PasswordConfirm { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (Password != PasswordConfirm)
                return new ValidationResult("密碼不相同");
            else
                return ValidationResult.Success;
        }
    }
}
