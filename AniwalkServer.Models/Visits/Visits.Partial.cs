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
        [Required(ErrorMessage = "請輸入內文")]
        public string MainText { get; set; } = null!;
        /// <summary>
        /// 經度
        /// </summary>
        [Display(Name = "經度")]
        [Required(ErrorMessage = "請於小地圖上點選座標")]
        public double Latitude { get; set; } = 0.0;
        /// <summary>
        /// 緯度
        /// </summary>
        [Display(Name = "緯度")]
        [Required(ErrorMessage = "請於小地圖上點選座標")]
        public double Longitude { get; set; } = 0.0;
        /// <summary>
        /// 到訪日期
        /// </summary>
        [Required(ErrorMessage = "請選擇到訪日期")]
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
        [Display(Name = "所在國家")]
        [Required(ErrorMessage = "請選擇所在國家")]
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
        [Required(ErrorMessage = "請選擇動畫")]
        public string AnimeID { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public virtual Animes? Anime { get; set; } = null!;

        /// <summary>
        /// 標籤
        /// </summary>
        public virtual List<VisitsDetails>? VisitDetail { get; set; }

        /// <summary>
        /// 到訪紀錄照片
        /// </summary>
        public virtual List<VisitsPhotos>? VisitsPhotos { get; set; } = null!;
        #endregion
    }
}
