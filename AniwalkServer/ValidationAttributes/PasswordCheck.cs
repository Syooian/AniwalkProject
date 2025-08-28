using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AniwalkServer.ValidationAttributes
{
    /// <summary>
    /// 密碼驗證
    /// </summary>
    public class PasswordCheck : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var Password = value as string;

            var StringLength = new StringLengthAttribute(20) { MinimumLength = 5 };
            var Regex = new RegularExpressionAttribute("^[a-zA-Z0-9]+$");

            //檢查是否有輸入
            if (string.IsNullOrEmpty(Password))
            {
                Debug.WriteLine("請輸入密碼");
                return new ValidationResult("請輸入密碼");
            }

            //檢查長度
            if (!StringLength.IsValid(Password))
            {
                Debug.WriteLine("密碼為5~20碼");
                return new ValidationResult("密碼為5~20碼");
            }

            //檢查內容
            if (!Regex.IsValid(Password))
            {
                Debug.WriteLine("密碼只能包含英文字母和數字");
                return new ValidationResult("密碼只能包含英文字母和數字");
            }

            return ValidationResult.Success;
        }
    }
}
