using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    public partial class Members
    {
        /// <summary>
        /// 會員ID
        /// </summary>
        [Key]
        [HiddenInput]
        [StringLength(10, MinimumLength = 10)]
        public string MemberID { get; set; } = null!;

        /// <summary>
        /// 會員名稱
        /// </summary>
        [StringLength(40)]
        [Display(Name = "會員名稱")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [EmailAddress]
        [StringLength(50)]
        [Display(Name = "電子郵件")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 創建日期
        /// </summary>
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 國家代碼
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Country))]
        [Display(Name = "所在國家")]
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// 國家
        /// </summary>
        public virtual Countries? Country { get; set; }

        /// <summary>
        /// 到訪紀錄
        /// </summary>
        public virtual List<Visits>? Visits { get; set; } = null!;
        #endregion
    }
}
