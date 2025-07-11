using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AniwalkServer.Models
{
    public partial class Animes
    {
        /// <summary>
        /// 動畫ID
        /// </summary>
        [Key]
        [HiddenInput]
        [StringLength(4, MinimumLength = 4)]
        public string AnimeID { get; set; } = null!;
        /// <summary>
        /// 名稱
        /// </summary>
        [StringLength(30)]
        public string Title { get; set; } = null!;
        /// <summary>
        /// 圖片(檔名)
        /// </summary>
        [StringLength(8, MinimumLength = 8)]
        public string HeaderPhoto { get; set; } = null!;
        /// <summary>
        /// 簡介
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        #region 外鍵關聯
        /// <summary>
        /// 到訪紀錄
        /// </summary>
        public virtual List<Visits>? Visits { get; set; }
        #endregion
    }
}
