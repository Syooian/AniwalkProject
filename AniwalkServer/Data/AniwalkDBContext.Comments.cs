using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;

namespace AniwalkServer.Data
{
    public partial class AniwalkDBContext
    {
        /// <summary>
        /// 評論
        /// </summary>
        public virtual DbSet<Comments> Comments { get; set; }
        /// <summary>
        /// 回覆
        /// </summary>
        public virtual DbSet<Replies> Replies { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder ModelBuilder)
        {

        }
    }
}
