using AniwalkServer.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AniwalkServer.ValidationAttributes
{
    /// <summary>
    /// 驗證密碼是否相同
    /// </summary>
    //[AttributeUsage(AttributeTargets.Class)]
    /*
     * ValidationAttribute預設只能標註在屬性上，而不是類別本身。如果你將 [NewPasswordCheck] 標註在 ForgotPasswordDTO 類別上，ASP.NET Core 預設不會自動執行這個類別級別的驗證，除非你明確指定這個屬性可以用在類別上。
     * 在 NewPasswordCheck 上加上 [AttributeUsage(AttributeTargets.Class)以允許標註在類別上
     */
    public class NewPasswordCheck : ValidationAttribute
    {
        /// <summary>
        /// 驗證密碼是否相同
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var FP = value as ForgotPasswordDTO;

            Debug.WriteLine("ForgotPasswordDTOCheck Phase : " + FP.Phase);

            if (FP.Phase == ForgotPasswordDTOPhase.ChangePassword)
            {
                var StringLength = new StringLengthAttribute(20) { MinimumLength = 5 };
                var Regex = new RegularExpressionAttribute("^[a-zA-Z0-9]+$");

                foreach (var PW in FP.NewPassword)
                {
                    //檢查是否有輸入
                    if (string.IsNullOrEmpty(PW))
                    {
                        Debug.WriteLine("請輸入密碼");
                        return new ValidationResult("請輸入密碼");
                    }

                    //檢查長度
                    if (!StringLength.IsValid(PW))
                    {
                        Debug.WriteLine("密碼為5~20碼");
                        return new ValidationResult("密碼為5~20碼");
                    }

                    //檢查內容
                    if (!Regex.IsValid(PW))
                    {
                        Debug.WriteLine("密碼只能包含英文字母和數字");
                        return new ValidationResult("密碼只能包含英文字母和數字");
                    }
                }

                //檢查密碼是否相同
                if (FP.NewPassword[0] != FP.NewPassword[1])
                {
                    Debug.WriteLine("輸入的密碼不相同");
                    return new ValidationResult("輸入的密碼不相同");
                }
            }

            return ValidationResult.Success;
        }
    }
}
