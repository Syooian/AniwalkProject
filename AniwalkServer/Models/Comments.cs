using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    public class Comments
    {
        /// <summary>
        /// 留言ID
        /// </summary>
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        public string CommentID { get; set; } = null!;

        /// <summary>
        /// 留言內容
        /// </summary>
        [StringLength(500)]
        [Display(Name = "留言內容")]
        public string CommentText { get; set; } = null!;

        /// <summary>
        /// 回覆日期
        /// </summary>
        [Display(Name = "回覆日期")]
        [HiddenInput]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 子留言集
        /// </summary>
        public virtual List<Comments>? ChildComments { get; set; }

        /// <summary>
        /// 回覆留言ID
        /// </summary>
        [HiddenInput]
        public string? ParentCommentID { get; set; }
        /// <summary>
        /// 回覆留言
        /// </summary>
        public virtual Comments? ParentComment { get; set; }

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
