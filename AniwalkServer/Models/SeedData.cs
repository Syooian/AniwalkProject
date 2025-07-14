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

                var Countries = new Countries[]
                {
                    new Countries() { CountryCode = "JPN", CountryName = "日本" },
                    new Countries() { CountryCode = "GBR", CountryName = "英國" },
                    new Countries() { CountryCode = "TWN", CountryName = "台灣" }
                };

                context.Countries.AddRange(Countries);
                #endregion

                #region 動畫資料
                if (context.Animes.Any())
                {
                    return;   // DB has been seeded
                }

                var Animes = new Animes[]
                {
                    new Animes() { AnimeID = "0001", Title = "聖誕之吻SS", HeaderPhoto = "0001.jpg", Description = "アマガミSS" },
                    new Animes() { AnimeID = "0002", Title = "K-ON！輕音部", HeaderPhoto = "0002.jpg", Description = "けいおん!" },
                    new Animes() { AnimeID = "0003", Title = "信長之槍", HeaderPhoto = "0003.jpg", Description = "ノブナガン" }
                };

                context.Animes.AddRange(Animes);
                #endregion

                #region 會員資料
                var MemberIDs = new string[2];
                for (int a = 0; a < MemberIDs.Length; a++)
                {
                    MemberIDs[a] = new Random().Next(0, 999999999).ToString("D10");

                    context.Members.Add(new Members()
                    {
                        MemberID = MemberIDs[a],
                        Name = "TestMember" + MemberIDs[a],
                        Email = MemberIDs[a] + "@example.com",
                        CountryCode = Countries[new Random().Next(0, Countries.Length)].CountryCode
                    });
                }
                #endregion

                #region 到訪紀錄資料
                if (context.Visits.Any())
                {
                    return;   // DB has been seeded
                }

                context.Visits.Add(new Visits()
                {
                    MainText = "七咲鞦韆",
                    Latitude = 35.725771,
                    Longitude = 140.819210,
                    MemberID = MemberIDs[0],
                    CountryCode = Countries[0].CountryCode,
                    AnimeID = Animes[0].AnimeID,
                    VisitedDate=DateTime.Now.AddDays(-5)
                });

                context.Visits.Add(new Visits()
                {
                    MainText = "七咲海岸",
                    Latitude = 35.706208,
                    Longitude = 140.837881,
                    MemberID = MemberIDs[0],
                    CountryCode = Countries[0].CountryCode,
                    AnimeID = Animes[1].AnimeID,
                    VisitedDate = DateTime.Now.AddDays(-10)
                });

                context.Visits.Add(new Visits()
                {
                    MainText = "怪獸襲來",
                    Latitude = 22.684911,
                    Longitude = 120.295731,
                    MemberID = MemberIDs[1],
                    CountryCode = Countries[2].CountryCode,
                    AnimeID = Animes[2].AnimeID,
                    VisitedDate = DateTime.Now.AddDays(-15)
                });
                #endregion

                context.SaveChanges();

                #region

                #endregion

                //context.SaveChanges();
            }
        }
    }
}
