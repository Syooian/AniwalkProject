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
    }
}
