using Microsoft.EntityFrameworkCore;
using AniwalkServer.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AniwalkServer.Data
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
                var SeedDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData");

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

                var Countries = new Countries[] { };

                //載入檔案
                {
                    var FilePath = Path.Combine(SeedDataPath, "Countries.txt");
                    if (Directory.Exists(SeedDataPath) && File.Exists(FilePath))
                    {
                        var DataStr = File.ReadAllText(FilePath);
                        Countries = JsonConvert.DeserializeObject<Countries[]>(DataStr);

                        //列出國名長度超出資料表限制的國家
                        foreach (var C in Countries)
                        {
                            if (C.CountryName.Length > 30)
                            {
                                Debug.WriteLine(C.CountryName + ", Length : " + C.CountryName.Length);
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Countries.txt路徑或檔案不存在");
                    }
                }

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

                #region 會員狀態
                context.MemberStatusCode.Add(new MemberStatusCode() { StatusCode = 0, StatusName = "一般" });
                context.MemberStatusCode.Add(new MemberStatusCode() { StatusCode = 1, StatusName = "已封鎖" });
                context.MemberStatusCode.Add(new MemberStatusCode() { StatusCode = 2, StatusName = "已註銷" });

                context.SaveChanges();
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
                        RoleID = 1,
                        MemberStatus = new MemberStatus()
                        {
                            MemberID = MemberIDs[a],
                            StatusCode = 0, //一般狀態
                        }
                    });

                    //一般會員帳密
                    context.Login.Add(new Login()
                    {
                        MemberID = MemberIDs[a],
                        Account = a + "2345678",
                        Password = a + "2345678"
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
                    RoleID = 9,
                    MemberStatus = new MemberStatus()
                    {
                        MemberID = AdminMemberID,
                        StatusCode = 0, //一般狀態
                    }
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
                var Visits = new List<Visits>();
                if (context.Visits.Any())
                {
                    return;   // DB has been seeded
                }

                Visits.Add(new Visits()
                {
                    MainText = "七咲鞦韆",
                    Latitude = 35.725771,
                    Longitude = 140.819210,
                    MemberID = MemberIDs[0],
                    CountryCode = Countries.FirstOrDefault(C => C.CountryCode == "JPN").CountryCode,
                    AnimeID = Animes[0].AnimeID,
                    VisitedDate = DateTime.Now.AddDays(-5)
                });

                Visits.Add(new Visits()
                {
                    MainText = "七咲海岸",
                    Latitude = 35.706208,
                    Longitude = 140.837881,
                    MemberID = MemberIDs[0],
                    CountryCode = Countries.FirstOrDefault(C => C.CountryCode == "JPN").CountryCode,
                    AnimeID = Animes[1].AnimeID,
                    VisitedDate = DateTime.Now.AddDays(-10)
                });

                Visits.Add(new Visits()
                {
                    MainText = "怪獸襲來",
                    Latitude = 22.684911,
                    Longitude = 120.295731,
                    MemberID = MemberIDs[1],
                    CountryCode = Countries.FirstOrDefault(C => C.CountryCode == "TWN").CountryCode,
                    AnimeID = Animes[2].AnimeID,
                    VisitedDate = DateTime.Now.AddDays(-15)
                });

                context.Visits.AddRange(Visits);

                context.SaveChanges();
                #endregion

                #region 到訪紀錄評論
                //因為SN是自動生成的，所以要先執行上方的資料新增後才能繼續執行
                if (context.Comments.Any())
                {
                    return;   // DB has been seeded
                }

                var Comments = new Comments[5];
                for (int a = 0; a < Comments.Length; a++)
                {
                    Comments[a] = new Comments()
                    {
                        CommentID = Guid.NewGuid().ToString(),
                        CommentDate = DateTime.Now.AddMinutes(-10 * (a + 1)),
                        MemberID = MemberIDs[0],
                        SN = 1,
                        CommentContent = "測試評論 for SN1 " + (a + 1)
                    };
                }

                context.AddRange(Comments);
                #endregion

                #region 到訪記錄評論的回覆
                var Replies = new List<Replies>();
                for (int a = 0; a < 3; a++)
                {
                    var Reply = new Replies()
                    {
                        ReplyID = Guid.NewGuid().ToString(),
                        CommentID = Comments[0].CommentID,
                        ReplyDate = DateTime.Now.AddMinutes(-5 * (a + 1)),
                        MemberID = MemberIDs[1],
                        ReplyContent = $"Reply for Comment {Comments[0].CommentID} {a + 1}"
                    };

                    Replies.Add(Reply);
                }

                //子回覆
                for (int a = 0; a < 2; a++)
                {
                    var Reply = new Replies()
                    {
                        ReplyID = Guid.NewGuid().ToString(),
                        CommentID = Comments[0].CommentID,
                        ReplyDate = DateTime.Now.AddMinutes(-4 * (a + 1)),
                        MemberID = MemberIDs[0],
                        ReplyContent = $"子回覆 for Reply {Replies[0].ReplyID} {a + 1}",
                        ParentReplyID = Replies[0].ReplyID
                    };

                    Replies.Add(Reply);
                }

                //子回覆的回覆
                Replies.Add(new Replies()
                {
                    ReplyID = Guid.NewGuid().ToString(),
                    CommentID = Comments[0].CommentID,
                    ReplyDate = DateTime.Now.AddMinutes(-3),
                    MemberID = MemberIDs[1],
                    ReplyContent = $"子回覆的回覆 for Reply {Replies[Replies.Count - 1].ReplyID}",
                    ParentReplyID = Replies[Replies.Count - 1].ReplyID
                });

                context.Replies.AddRange(Replies);
                #endregion

                #region 到訪照片
                if (!context.VisitsPhotos.Any())
                {
                    #region 刪除現有照片
                    string[] Files;
                    if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath)))
                    {
                        Files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath),
                           "*",//所有檔案
                           SearchOption.AllDirectories);//所有子資料夾
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
                    }
                    #endregion

                    string VisitsPhotosRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Shared.VisitsPhotosRootPath);//複製檔案的目的路徑
                    if (!Directory.Exists(VisitsPhotosRootPath))
                        Directory.CreateDirectory(VisitsPhotosRootPath); //如果目的Root路徑不存在，則建立

                    for (int a = 0; a < Visits.Count; a++)
                    {
                        var SN = a + 1;
                        var SeedPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedPhotos", "Visits", SN.ToString());//每個到訪記錄對應的SeedData照片路徑

                        if (Directory.Exists(SeedPhotosPath))
                        {
                            Files = Directory.GetFiles(SeedPhotosPath);  //取得SeedData路徑中的所有檔案

                            //檢查該使用者的照片路徑是否存在
                            var VisitsPhotosPath = Path.Combine(VisitsPhotosRootPath, Visits[a].MemberID);
                            if (!Directory.Exists(VisitsPhotosPath))
                                Directory.CreateDirectory(VisitsPhotosPath);

                            for (int b = 0; b < Files.Length; b++)
                            {
                                //建立資料
                                var VP = new VisitsPhotos()
                                {
                                    MemberID = Visits[a].MemberID,
                                    PhotoID = Guid.NewGuid().ToString(),
                                    PhotoType = Path.GetExtension(Files[b]),
                                    Description = Visits[a].MainText + " Photo " + (b + 1),
                                    SN = SN,
                                    SortNumber = b
                                };
                                //加入資料表
                                context.VisitsPhotos.Add(VP);

                                //複製圖片檔案到wwwroot
                                string ToFile = Path.Combine(VisitsPhotosPath, VP.PhotoID + ".jpg");
                                File.Copy(Files[b], ToFile);
                            }
                        }
                    }

                    context.SaveChanges();
                }
                #endregion

                #region 公告
                if (!context.Announcements.Any())
                {
                    context.Announcements.Add(new Announcements()
                    {
                        Title = "歡迎使用 AniWalk！",
                        Content = "AniWalk 是一個專為動漫迷設計的到訪紀錄平台，快來分享你的動漫之旅吧！",
                        CreatedDate = DateTime.Now.AddMonths(-1)
                    });

                    context.Announcements.Add(new Announcements()
                    {
                        Title = "Announcement Test 1",
                        Content = "This is a test announcement content for testing purposes.",
                        CreatedDate = DateTime.Now.AddDays(-10)
                    });

                    context.SaveChanges();
                }
                #endregion

                // 開啟 IDENTITY_INSERT (自行填入ID)
                //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT VisitsTags ON");

                context.SaveChanges();
            }
        }
    }
}
