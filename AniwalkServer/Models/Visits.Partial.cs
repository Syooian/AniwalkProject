using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 到訪紀錄
    /// </summary>
    public partial class Visits
    {
        /// <summary>
        /// 序號
        /// </summary>
        [Key]
        [HiddenInput]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SN { get; set; }
        /// <summary>
        /// 內文
        /// </summary>
        [StringLength(1000)]
        [Display(Name = "內文")]
        [DataType(DataType.MultilineText)]//標註多行文字
        public string MainText { get; set; } = null!;
        /// <summary>
        /// 經度
        /// </summary>
        public double Latitude { get; set; } = 0.0;
        /// <summary>
        /// 緯度
        /// </summary>
        public double Longitude { get; set; } = 0.0;
        /// <summary>
        /// 到訪日期
        /// </summary>
        [Required]
        [Display(Name = "到訪日期")]
        public DateTime VisitedDate { get; set; }
        /// <summary>
        /// 創建日期
        /// </summary>
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Member))]
        public string MemberID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Members? Member { get; set; } = null!;

        /// <summary>
        /// 國家代碼
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Country))]
        [Display(Name = "到訪國家")]
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// 國家
        /// </summary>
        public virtual Countries? Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        [ForeignKey(nameof(Anime))]
        [Display(Name = "動畫")]
        public string AnimeID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Animes? Anime { get; set; } = null!;

        /// <summary>
        /// 標籤
        /// </summary>
        public virtual List<VisitsTags>? VisitsTag { get; set; }

        /// <summary>
        /// 到訪紀錄照片
        /// </summary>
        public virtual List<VisitsPhotos>? VisitsPhotos { get; set; } = null!;
        #endregion
    }
}
