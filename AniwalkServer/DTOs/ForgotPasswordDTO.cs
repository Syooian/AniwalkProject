using AniwalkServer.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using static AniwalkServer.ValidationAttributes.NewPasswordCheck;

namespace AniwalkServer.DTOs
{
    public class ForgotPasswordDTO : IValidatableObject
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
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "請輸入密碼")]
        //[StringLength(20, MinimumLength = 5, ErrorMessage = "密碼為5~20碼")]
        //[RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "密碼只能包含英文字母和數字")]
        //在陣列的情況下屬性驗證只會檢查陣列是否為Null，不會檢查陣列內的每個值
        public string[]? NewPassword { get; set; }

        /// <summary>
        /// 驗證階段
        /// </summary>
        public ForgotPasswordDTOPhase Phase { get; set; } = ForgotPasswordDTOPhase.Email;

        /// <summary>
        /// 驗證輸入密碼
        /// </summary>
        /// <param name="ValidationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext ValidationContext)
        {
            Debug.WriteLine("ForgotPasswordDTOCheck Phase : " + Phase);

            if (Phase == ForgotPasswordDTOPhase.ChangePassword)
            {
                var StringLength = new StringLengthAttribute(20) { MinimumLength = 5 };
                var Regex = new RegularExpressionAttribute("^[a-zA-Z0-9]+$");

                foreach (var PW in NewPassword)
                {
                    //檢查是否有輸入
                    if (string.IsNullOrEmpty(PW))
                    {
                        Debug.WriteLine("請輸入密碼");
                        yield return new ValidationResult("請輸入密碼");
                    }

                    //檢查長度
                    if (!StringLength.IsValid(PW))
                    {
                        Debug.WriteLine("密碼為5~20碼");
                        yield return new ValidationResult("密碼為5~20碼");
                    }

                    //檢查內容
                    if (!Regex.IsValid(PW))
                    {
                        Debug.WriteLine("密碼只能包含英文字母和數字");
                        yield return new ValidationResult("密碼只能包含英文字母和數字");
                    }
                }

                //檢查密碼是否相同
                if (NewPassword[0] != NewPassword[1])
                {
                    Debug.WriteLine("輸入的密碼不相同");
                    yield return new ValidationResult("輸入的密碼不相同");
                }
            }

            yield return ValidationResult.Success;
        }
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
