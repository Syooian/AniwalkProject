using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models.ForgotPassword
{
    /// <summary>
    /// 忘記密碼
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        [HiddenInput]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SN { get; set; }
        /// <summary>
        /// 驗證碼有效期限
        /// </summary>
        [Display(Name = "驗證碼有效期限")]
        public DateTime VerifyCodeExpiryDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 建立日期
        /// </summary>
        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 驗證碼
        /// <para>ForgotPasswordDTO裡也有一個VerifyCode，如果將來要修改驗證規則須連那個一起改</para>
        /// </summary>
        [Display(Name = "驗證碼")]
        [Column(TypeName = "char")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "驗證碼長度必須為5個字元")]
        public string VerifyCode { get; set; } = null!;

        #region 外鍵關聯
        /// <summary>
        /// 會員ID
        /// </summary>
        [Display(Name = "會員ID")]
        [ForeignKey(nameof(Member))]
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 會員
        /// </summary>
        public virtual Members? Member { get; set; } = null!;
        #endregion
    }
}
