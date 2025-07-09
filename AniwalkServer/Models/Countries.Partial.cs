using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AniwalkServer.Models
{
    public partial class Countries
    {
        /// <summary>
        /// 國家代碼
        /// </summary>
        [Key]
        [HiddenInput]
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// 國名
        /// </summary>
        public string CountryName { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
