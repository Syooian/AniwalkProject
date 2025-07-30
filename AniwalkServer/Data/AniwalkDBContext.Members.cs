using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using System.Diagnostics;

namespace AniwalkServer.Data
{
    public partial class AniwalkDBContext
    {
        /// <summary>
        /// 會員狀態
        /// </summary>
        public virtual DbSet<MemberStatus> MemberStatus { get; set; }

        //protected partial void OnModelCreatingPartial(ModelBuilder ModelBuilder)
        //{

        //}
    }
}
