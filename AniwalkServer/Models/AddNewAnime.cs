using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 新增動畫建議
    /// </summary>
    public class AddNewAnime
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        [HiddenInput]
        public int SN { get; set; }
        /// <summary>
        /// 動畫標題
        /// </summary>
        [Display(Name = "動畫標題")]
        [StringLength(50)]
        [Required(ErrorMessage = "請輸入動畫標題")]
        public string AnimeTitle { get; set; } = null!;
        /// <summary>
        /// 新增日期
        /// </summary>
        [Display(Name = "新增日期")]
        public DateTime AddDate { get; set; }
        /// <summary>
        /// 表單狀態
        /// </summary>
        [Display(Name = "表單狀態")]
        [Column(TypeName = "tinyint")]
        public AddNewAnimeStatusEnum Status { get; set; } = AddNewAnimeStatusEnum.NotYetProcessed;
        /// <summary>
        /// 表單完結日期
        /// </summary>
        [Display(Name = "表單完結日期")]
        public DateTime? CloseDate { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(50)]
        public string? Note { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AddNewAnimeStatusEnum
    {
        /// <summary>
        /// 尚未處理
        /// </summary>
        [Display(Name = "尚未處理")]
        NotYetProcessed = 0,
        /// <summary>
        /// 審核中
        /// </summary>
        [Display(Name = "審核中")]
        InProgress = 1,
        /// <summary>
        /// 同意新增
        /// </summary>
        [Display(Name = "同意新增")]
        AgreeToAdd = 2,
        /// <summary>
        /// 不同意新增
        /// </summary>
        [Display(Name = "不同意新增")]
        Disagree = 3
    }
}
