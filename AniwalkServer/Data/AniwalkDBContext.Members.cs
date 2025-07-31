using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;

namespace AniwalkServer.Data
{
    public partial class AniwalkDBContext
    {
        /// <summary>
        /// 會員狀態
        /// </summary>
        public virtual DbSet<MemberStatus> MemberStatus { get; set; }
        /// <summary>
        /// 會員狀態碼
        /// </summary>
        public virtual DbSet<MemberStatusCode> MemberStatusCode { get; set; }

        //protected partial void OnModelCreatingPartial(ModelBuilder ModelBuilder)
        //{

        //}
    }
}
