using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace AniwalkServer.ValidationAttributes
{
    public class EmailCheck : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var Email = value as string;

            #region 驗證規則
            var IsEmailErrorMessage = "請輸入正確的電子郵件格式";
            var IsEmail = new EmailAddressAttribute()
            {
                ErrorMessage = IsEmailErrorMessage
            };

            var StringLength = new StringLengthAttribute(50);
            #endregion

            //檢查是否有輸入
            if (string.IsNullOrEmpty(Email))
            {
                Debug.WriteLine("請輸入電子郵件");
                return new ValidationResult("請輸入電子郵件");
            }

            //驗證是否是Email格式
            if (!IsEmail.IsValid(Email))
                return new ValidationResult(IsEmailErrorMessage);

            if (!StringLength.IsValid(Email))
                return new ValidationResult("Email太長");

            return ValidationResult.Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        public void AddValidation(ClientModelValidationContext Context)
        {

        }
    }
}
