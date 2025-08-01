//using AniwalkServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 檢查輸入的會員名稱是否已被使用
    /// </summary>
    public class CheckName : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberName"></param>
        /// <param name="ValidationContext"></param>
        /// <returns></returns>
        protected override ValidationResult? IsValid(object? MemberName, ValidationContext ValidationContext)
        {
            var Services = ValidationContext.GetService<MemberValidationService>();
            if (Services == null)
            {
                throw new InvalidOperationException("MemberValidationService 未注入。");
            }

            if (Services.IsNameTaken(MemberName.ToString()))
            {
                return new ValidationResult("此會員名稱已被使用，請選擇其他名稱。");
            }

            return ValidationResult.Success;
        }
    }
}
