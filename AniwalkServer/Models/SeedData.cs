using Microsoft.EntityFrameworkCore;

namespace AniwalkServer.Models
{
    public class SeedData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceProvider"></param>
        public static void Initialize(IServiceProvider ServiceProvider)
        {
            using (var context = new AniwalkDBContext(
                ServiceProvider.GetRequiredService<DbContextOptions<AniwalkDBContext>>()))
            {
                #region 國家資料
                if (context.Countries.Any())
                {
                    return;   // DB has been seeded
                }

                context.Countries.Add(new Countries()
                {
                    CountryCode = "JPN",
                    CountryName = "日本",
                });
                context.Countries.Add(new Countries()
                {
                    CountryCode = "GBR",
                    CountryName = "英國",
                });
                context.Countries.Add(new Countries()
                {
                    CountryCode = "TWN",
                    CountryName = "台灣",
                });
                #endregion

                #region 動畫資料
                if (context.Animes.Any())
                {
                    return;   // DB has been seeded
                }

                context.Animes.Add(new Animes()
                {
                    AnimeID = "0001",
                    Title = "聖誕之吻SS",
                    HeaderPhoto = "0001.jpg",
                    Description = "アマガミSS"
                });
                context.Animes.Add(new Animes()
                {
                    AnimeID = "0002",
                    Title = "K-ON！輕音部",
                    HeaderPhoto = "0002.jpg",
                    Description = "けいおん!"
                });
                context.Animes.Add(new Animes()
                {
                    AnimeID = "0003",
                    Title = "信長之槍",
                    HeaderPhoto = "0003.jpg",
                    Description = "ノブナガン"
                });
                #endregion

                context.SaveChanges();

                #region 動畫與國家的關聯
                /*
                 *聖誕之吻
                 *0001 : JPN
                 */
                context.Database.ExecuteSqlRaw(@"insert into AnimesCountries values
                    ('0001', 'JPN')
                ");

                /*
                 *K-ON
                 *0002 : JPN
                 *0002 : GBR
                 */
                context.Database.ExecuteSqlRaw(@"insert into AnimesCountries values
                    ('0002', 'JPN'),
                    ('0002', 'GBR')
                ");

                /*信長之槍
                *0003 : JPN
                *0003 : TWN
                */
                context.Database.ExecuteSqlRaw(@"insert into AnimesCountries values
                    ('0003', 'JPN'),
                    ('0003', 'TWN')
                ");
                #endregion

                context.SaveChanges();
            }
        }
    }
}
