using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 回覆
    /// </summary>
    public class Replies
    {
        /// <summary>
        /// 回覆ID
        /// <para>GUID</para>
        /// </summary>
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        [Column(TypeName = "char(36)")]
        public string ReplyID { get; set; } = null!;

        /// <summary>
        /// 回覆內容
        /// </summary>
        [StringLength(500)]
        [Display(Name = "回覆內容")]
        [Required(ErrorMessage = "請輸入回覆內容")]
        public string ReplyContent { get; set; } = null!;

        /// <summary>
        /// 回覆日期
        /// </summary>
        [Display(Name = "回覆日期")]
        [HiddenInput]
        public DateTime ReplyDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 刪除日期
        /// </summary>
        [Display(Name = "刪除日期")]
        public DateTime? DeleteDate { get; set; }

        #region 外鍵關聯
        /// <summary>
        /// 評論ID
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Comment))]
        public string CommentID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Comments? Comment { get; set; } = null!;

        /// <summary>
        /// 父回覆ID
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(ParentReply))]
        public string? ParentReplyID { get; set; } = null!;
        /// <summary>
        /// 父回覆
        /// </summary>
        [HiddenInput]
        public virtual Replies? ParentReply { get; set; } = null!;

        /// <summary>
        /// 子回覆
        /// </summary>
        public virtual List<Replies>? ChildrenReplies { get; set; }

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
