using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 會員狀態碼
    /// </summary>
    public partial class MemberStatusCode
    {
        /// <summary>
        /// 狀態碼
        /// </summary>
        [Key]
        [HiddenInput]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 禁用自動生成，避免EF Core自動設為Indentity
        public int StatusCode { get; set; } = 0;
        /// <summary>
        /// 狀態名稱
        /// </summary>
        [Display(Name = "狀態名稱")]
        [StringLength(10)]
        public string StatusName { get; set; } = null!;
        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(20)]
        public string? Note { get; set; }

        #region 外鍵關聯
        /// <summary>
        /// 
        /// </summary>
        public virtual List<MemberStatus>? MemberStatus { get; set; } = null!;
        #endregion
    }
}
