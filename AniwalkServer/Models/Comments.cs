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
        /// 回覆日期
        /// </summary>
        [Display(Name = "回覆日期")]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 子留言集
        /// </summary>
        public virtual List<Comments>? ChildComments { get; set; }

        /// <summary>
        /// 回覆留言ID
        /// </summary>
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
        #endregion
    }
}
