using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AniwalkServer.Models
{
    public partial class Countries
    {
        /// <summary>
        /// 國家代碼
        /// </summary>
        [Key]
        [HiddenInput]
        [StringLength(3, MinimumLength = 3)]
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// 國名
        /// </summary>
        [StringLength(30)]
        public string CountryName { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 會員
        /// </summary>
        public virtual List<Members>? Members { get; set; } = null!;
        /// <summary>
        /// 到訪紀錄
        /// </summary>
        public virtual List<Visits>? Visits { get; set; } = null!;
        #endregion
    }
}
