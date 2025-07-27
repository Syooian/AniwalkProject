using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using System.Diagnostics;

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

        protected partial void OnModelCreatingPartial(ModelBuilder ModelBuilder)
        {
            /*
             * 在資料表 'Replies' 導入 FOREIGN KEY 條件約束 'FK_Replies_Members_MemberID' 可能造成循環或多個串聯路徑。
             * 請指定 ON DELETE NO ACTION 或 ON UPDATE NO ACTION，或者修改其他 FOREIGN KEY 條件約束
             * 
             * 此問題是由於在 Replies 資料表中新增的外鍵條件約束 FK_Replies_Members_MemberID 與其他外鍵條件約束之間可能存在循環或多個串聯路徑所導致的。
             * 
             * 解決方案
                在 InitialCreate 遷移檔案中，修改 FK_Replies_Members_MemberID 的行為：

                修改遷移檔案
                將 FK_Replies_Members_MemberID 的 onDelete 行為設為 Restrict 或 NoAction。
             */
            ModelBuilder.Entity<Replies>()
                .HasOne(R => R.Member)
                .WithMany()
                .HasForeignKey(R => R.MemberID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
