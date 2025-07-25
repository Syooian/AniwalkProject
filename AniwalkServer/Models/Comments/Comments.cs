using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 評論
    /// </summary>
    public class Comments
    {
        /// <summary>
        /// 留言ID
        /// <para>GUID</para>
        /// </summary>
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        [Column(TypeName = "char(36)")]
        public string CommentID { get; set; } = null!;

        /// <summary>
        /// 評論內容
        /// </summary>
        [StringLength(500)]
        [Display(Name = "評論內容")]
        public string CommentContent { get; set; } = null!;

        /// <summary>
        /// 評論日期
        /// </summary>
        [Display(Name = "評論日期")]
        [HiddenInput]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 回覆
        /// </summary>
        public virtual List<Replies>? Replies { get; set; }

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
        public virtual Visits? Visit { get; set; }
        #endregion
    }
}
