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
    public class ForgotPasswordDTOCheck : ValidationAttribute
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
                Debug.WriteLine($"P1 : {FP.NewPassword}, P2 : {FP.NewPasswordConfirm}");

                //檢查密碼是否相同
                if (FP.NewPassword != FP.NewPasswordConfirm)
                {
                    Debug.WriteLine("輸入的密碼不相同");
                    return new ValidationResult("輸入的密碼不相同");
                }
            }

            return ValidationResult.Success;
        }
    }
}
