using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Models
{
    public class AniwalkDBContext : DbContext
    {
        public AniwalkDBContext(DbContextOptions<AniwalkDBContext> Options) : base(Options)
        {

        }

        #region 描述資料表
        /// <summary>
        /// 國家
        /// </summary>
        public virtual DbSet<Countries> Countries { get; set; }
        #endregion

        /// <summary>
        /// 資料庫模型進階設定
        /// </summary>
        /// <param name="ModelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Countries>(Entity =>
            {
                Entity.Property(E => E.CountryCode)
                    //.IsUnicode(false);//True時資料庫值為nvarchar，False時為varchar
                    .HasColumnType("char");
            });
        }
    }
}
