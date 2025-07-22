using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    public partial class VisitsTags
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        [HiddenInput]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SN { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        [StringLength(10)]
        public string Tag { get; set; } = null!;

        #region 外鍵關聯
        /// <summary>
        /// 標籤
        /// </summary>
        public virtual List<VisitsDetails>? VisitDetail { get; set; } = null!;
        #endregion
    }
}
