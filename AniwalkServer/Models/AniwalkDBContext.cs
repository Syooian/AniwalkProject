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
        /// <summary>
        /// 動畫
        /// </summary>
        public virtual DbSet<Animes> Animes { get; set; }
        /// <summary>
        /// 會員
        /// </summary>
        public virtual DbSet<Members> Members { get; set; }
        /// <summary>
        /// 到訪紀錄
        /// </summary>
        public virtual DbSet<Visits> Visits { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public virtual DbSet<Comments> Comments { get; set; }
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

            ModelBuilder.Entity<Animes>(Entity =>
            {
                Entity.Property(E => E.AnimeID)
                    .HasColumnType("char");
                Entity.Property(E => E.HeaderPhoto)
                    .HasColumnType("char");
            });

            ModelBuilder.Entity<Members>(Entity =>
            {
                Entity.Property(E => E.MemberID)
                    .HasColumnType("char");

                Entity.Property(E => E.Email)
                    .IsUnicode(false); // Email通常不需要Unicode，使用ASCII即可.
            });

            ModelBuilder.Entity<Visits>(Entity =>
            {
                /**
                   在 Visits 資料表上建立 MemberID 的外鍵時，因為與其他外鍵（如 CountryCode）之間的關聯
                   導致 SQL Server 偵測到「循環」或「多重串聯路徑」的情況。這通常發生在多個表之間有重複的外鍵路徑，且這些外鍵都設定了 ON DELETE CASCADE 或 ON UPDATE CASCADE。
                   解決方式：
                   需要在 Fluent API 或 Migration 中，明確指定 ON DELETE NO ACTION 或 ON UPDATE NO ACTION，避免自動連鎖刪除或更新。
                */
                Entity.
                    HasOne(E => E.Member)
                    .WithMany(E => E.Visits)
                    .HasForeignKey(E => E.MemberID)
                    .OnDelete(DeleteBehavior.NoAction);
                Entity
                    .HasOne(E => E.Country)
                    .WithMany(E => E.Visits)
                    .HasForeignKey(E => E.CountryCode)
                    .OnDelete(DeleteBehavior.NoAction);
                Entity
                    .HasOne(E => E.Anime)
                    .WithMany(E => E.Visits)
                    .HasForeignKey(E => E.AnimeID)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
