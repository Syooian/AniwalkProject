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
                #region 新增動畫建議
                if (!context.AddNewAnimes.Any())
                {
                    context.AddNewAnimes.AddRange(
                        new AddNewAnime[]
                        {
                            new AddNewAnime() { AnimeTitle = "新動畫建議1", AddDate=DateTime.Now},
                            new AddNewAnime() { AnimeTitle = "新動畫建議2", AddDate=DateTime.Now.AddMonths(-1),Status=AddNewAnimeStatusEnum.InProgress},
                            new AddNewAnime() { AnimeTitle = "新動畫建議3", AddDate=DateTime.Now.AddMonths(-2),Status=AddNewAnimeStatusEnum.AgreeToAdd, CloseDate=DateTime.Now.AddDays(-1) },
                            new AddNewAnime() { AnimeTitle = "新動畫建議4", AddDate=DateTime.Now.AddMonths(-3),Status=AddNewAnimeStatusEnum.Disagree, CloseDate=DateTime.Now.AddDays(-2), Note="已有該動畫" }
                        });
                }
                #endregion

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

                #region 會員角色資料
                if (!context.MemberRoles.Any())//資料表無資料時才執行
                {
                    var MemberRoles = new MemberRoles[]
                    {
                        new MemberRoles() { RoleID = 0, RoleName = "訪客" },
                        new MemberRoles() { RoleID = 1, RoleName = "一般會員" },
                        new MemberRoles() { RoleID = 9, RoleName = "管理員" }
                    };

                    context.MemberRoles.AddRange(MemberRoles);

                    context.SaveChanges();
                }
                #endregion

                #region 會員資料
                if (context.Members.Any())
                {
                    return;   // DB has been seeded
                }

                //一般會員
                var MemberIDs = new string[2];
                for (int a = 0; a < MemberIDs.Length; a++)
                {
                    MemberIDs[a] = new Random().Next(0, 999999999).ToString("D10");

                    context.Members.Add(new Members()
                    {
                        MemberID = MemberIDs[a],
                        Name = "TestMember" + MemberIDs[a],
                        Email = MemberIDs[a] + "@example.com",
                        CountryCode = Countries[new Random().Next(0, Countries.Length)].CountryCode,
                        RoleID = 1
                    });
                }

                //一般會員帳密 (僅一個)
                {
                    context.Login.Add(new Login()
                    {
                        MemberID = MemberIDs[0],
                        Account = "12345678",
                        Password = "12345678"
                    });
                }

                //管理員
                var AdminMemberID = "0999999999";
                context.Members.Add(new Members()
                {
                    MemberID = AdminMemberID,
                    Name = "かんりいん",
                    Email = "Admin@example.com",
                    CountryCode = Countries[new Random().Next(0, Countries.Length)].CountryCode,
                    RoleID = 9
                });

                //管理員帳密
                context.Login.Add(new Login()
                {
                    MemberID = AdminMemberID,
                    Account = "Admin",
                    Password = "Admin"
                });
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
                    VisitedDate = DateTime.Now.AddDays(-5)
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

                #region 到訪紀錄留言
                //因為SN是自動生成的，所以要先執行上方的資料新增後才能繼續執行
                if (context.Comments.Any())
                {
                    return;   // DB has been seeded
                }

                for (int a = 0; a < 5; a++)
                {
                    context.Comments.Add(new Comments()
                    {
                        CommentID = Guid.NewGuid().ToString(),
                        CommentDate = DateTime.Now.AddMinutes(-5 * (a + 1)),
                        MemberID = MemberIDs[0],
                        SN = 1,
                        CommentContent = "測試留言 for 七咲鞦韆 " + (a + 1)
                    });
                }
                #endregion

                #region 到訪照片
                if (!context.VisitsPhotos.Any())
                {
                    #region 刪除現有照片
                    string[] Files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VisitsPhotos"));
                    for (int a = 0; a < Files.Length; a++)
                    {
                        try
                        {
                            File.Delete(Files[a]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting file {Files[a]}: {ex.Message}");
                        }
                    }
                    #endregion

                    var VPs = new VisitsPhotos[16];

                    for (int a = 1; a <= 12; a++)
                    {
                        VPs[a - 1] = new VisitsPhotos()
                        {
                            MemberID = MemberIDs[0],
                            PhotoID = Guid.NewGuid().ToString(),
                            Description = "七咲鞦韆 Photo " + a,
                            SN = 1
                        };

                        VPs[a + 3] = new VisitsPhotos()
                        {
                            MemberID = MemberIDs[0],
                            PhotoID = Guid.NewGuid().ToString(),
                            Description = "怪獸襲來 Photo " + a,
                            SN = 2
                        };
                    }

                    context.VisitsPhotos.AddRange(VPs);

                    context.SaveChanges();

                    #region 照片轉存
                    string SeedPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedPhotos", "Visits");//取得來源照片路徑
                    string VisitsPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VisitsPhotos");//目的路徑
                    Files = Directory.GetFiles(SeedPhotosPath);  //取得指定路徑中的所有檔案

                    if (!Directory.Exists(VisitsPhotosPath))
                    {
                        Directory.CreateDirectory(VisitsPhotosPath); //如果目的路徑不存在，則建立
                    }

                    for (int a = 0; a < VPs.Length; a++)
                    {
                        string ToFile = Path.Combine(VisitsPhotosPath, VPs[a].PhotoID + ".jpg");

                        File.Copy(Files[a], ToFile);
                    }
                    #endregion
                }
                #endregion

                // 開啟 IDENTITY_INSERT (自行填入ID)
                //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT VisitsTags ON");

                context.SaveChanges();
            }
        }
    }
}
