using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    /// <summary>
    /// 到訪紀錄明細
    /// </summary>
    [PrimaryKey(nameof(TagSN), nameof(VisitSN))]
    public class VisitsDetails
    {
        #region 外鍵關聯
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(VisitsTag))]
        public int TagSN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual VisitsTags? VisitsTag { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(Visit))]
        public int VisitSN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Visits? Visit { get; set; } = null!;
        #endregion
    }
}
