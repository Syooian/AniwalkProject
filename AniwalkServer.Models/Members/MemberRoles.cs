using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniwalkServer.Models
{
    public class MemberRoles
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [HiddenInput]
        [Key]
        [Range(0, 9)]
        [Column(TypeName = "tinyint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // 禁用自動生成，避免EF Core自動設為Indentity
        public int RoleID { get; set; } = 0;
        /// <summary>
        /// 角色名稱
        /// </summary>
        [StringLength(10)]
        [Display(Name = "角色名稱")]
        public string RoleName { get; set; } = null!;

        #region 外鍵關聯
        /// <summary>
        /// 會員
        /// </summary>
        public virtual List<Members>? Members { get; set; } = null!;
        #endregion
    }
}
