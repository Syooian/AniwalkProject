using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 公告
    /// </summary>
    public class Announcements
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        [HiddenInput]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SN { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        [Display(Name = "標題")]
        [StringLength(30)]
        public string Title { get; set; } = null!;
        /// <summary>
        /// 內容
        /// </summary>
        [Display(Name = "內容")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)] // 標註多行文字
        public string Content { get; set; } = null!;
        /// <summary>
        /// 建立日期
        /// </summary>
        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
