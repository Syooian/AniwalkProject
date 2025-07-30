using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
    /// 會員狀態
    /// </summary>
    public partial class MemberStatus
    {
        /// <summary>
        /// 更新時間
        /// </summary>
        [Display(Name = "更新時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]//由資料庫自動更新
        public DateTime UpdateDate { get; set; } = DateTime.Now;

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
        [Key]
        [ForeignKey(nameof(Member))]
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Members? Member { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(MemberStatusCode))]
        public int StatusCode { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public virtual MemberStatusCode? MemberStatusCode { get; set; } = null!;
        #endregion
    }
}
