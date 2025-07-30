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
        public virtual DbSet<MemberStatusCode> MemberStatus { get; set; }

        //protected partial void OnModelCreatingPartial(ModelBuilder ModelBuilder)
        //{

        //}
    }
}
