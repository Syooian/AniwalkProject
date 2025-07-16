using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 到訪紀錄照片
    /// </summary>
    public class VisitsPhotos
    {
        /// <summary>
        /// 照片ID
        /// </summary>
        [HiddenInput]
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "char(36)")]
        [Key]
        public string PhotoID { get; set; } = null!;
        /// <summary>
        /// 說明
        /// </summary>
        [Display(Name = "說明", Description = "照片的說明")]
        [StringLength(500)]
        public string Description { get; set; } = null!;

        #region 外鍵關聯
        /// <summary>
        /// 會員
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Member))]
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 會員
        /// </summary>
        public virtual Members? Member { get; set; } = null!;

        /// <summary>
        /// 到訪紀錄
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Visit))]
        public int SN { get; set; }
        /// <summary>
        /// 到訪紀錄
        /// </summary>
        public virtual Visits? Visit { get; set; } = null!;
        #endregion
    }
}
