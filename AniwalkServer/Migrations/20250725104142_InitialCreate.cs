using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddNewAnimes",
                columns: table => new
                {
                    SN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Status = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddNewAnimes", x => x.SN);
                });

            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    AnimeID = table.Column<string>(type: "char(4)", maxLength: 4, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    HeaderPhoto = table.Column<string>(type: "char(8)", maxLength: 8, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.AnimeID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "char(3)", maxLength: 3, nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    RoleID = table.Column<byte>(type: "tinyint", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "VisitsTags",
                columns: table => new
                {
                    SN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tag = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitsTags", x => x.SN);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "char(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountryCode = table.Column<string>(type: "char(3)", nullable: false),
                    RoleID = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_MemberRoles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "MemberRoles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Account = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Account);
                    table.ForeignKey(
                        name: "FK_Login_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visits",
                columns: table => new
                {
                    SN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    VisitedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false),
                    CountryCode = table.Column<string>(type: "char(3)", nullable: false),
                    AnimeID = table.Column<string>(type: "char(4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visits", x => x.SN);
                    table.ForeignKey(
                        name: "FK_Visits_Animes_AnimeID",
                        column: x => x.AnimeID,
                        principalTable: "Animes",
                        principalColumn: "AnimeID");
                    table.ForeignKey(
                        name: "FK_Visits_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode");
                    table.ForeignKey(
                        name: "FK_Visits_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    CommentContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false),
                    SN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comments_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Visits_SN",
                        column: x => x.SN,
                        principalTable: "Visits",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitsDetails",
                columns: table => new
                {
                    TagSN = table.Column<int>(type: "int", nullable: false),
                    VisitSN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitsDetails", x => new { x.TagSN, x.VisitSN });
                    table.ForeignKey(
                        name: "FK_VisitsDetails_VisitsTags_TagSN",
                        column: x => x.TagSN,
                        principalTable: "VisitsTags",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitsDetails_Visits_VisitSN",
                        column: x => x.VisitSN,
                        principalTable: "Visits",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitsPhotos",
                columns: table => new
                {
                    PhotoID = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false),
                    SN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitsPhotos", x => x.PhotoID);
                    table.ForeignKey(
                        name: "FK_VisitsPhotos_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitsPhotos_Visits_SN",
                        column: x => x.SN,
                        principalTable: "Visits",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    ReplyID = table.Column<string>(type: "char(36)", maxLength: 36, nullable: false),
                    ReplyContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentID = table.Column<string>(type: "char(36)", nullable: false),
                    ParentReplyID = table.Column<string>(type: "char(36)", nullable: true),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.ReplyID);
                    table.ForeignKey(
                        name: "FK_Replies_Comments_CommentID",
                        column: x => x.CommentID,
                        principalTable: "Comments",
                        principalColumn: "CommentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replies_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID");
                    table.ForeignKey(
                        name: "FK_Replies_Replies_ParentReplyID",
                        column: x => x.ParentReplyID,
                        principalTable: "Replies",
                        principalColumn: "ReplyID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MemberID",
                table: "Comments",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SN",
                table: "Comments",
                column: "SN");

            migrationBuilder.CreateIndex(
                name: "IX_Login_MemberID",
                table: "Login",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_CountryCode",
                table: "Members",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_Name",
                table: "Members",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_RoleID",
                table: "Members",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CommentID",
                table: "Replies",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_MemberID",
                table: "Replies",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ParentReplyID",
                table: "Replies",
                column: "ParentReplyID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_AnimeID",
                table: "Visits",
                column: "AnimeID");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_CountryCode",
                table: "Visits",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_MemberID",
                table: "Visits",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitsDetails_VisitSN",
                table: "VisitsDetails",
                column: "VisitSN");

            migrationBuilder.CreateIndex(
                name: "IX_VisitsPhotos_MemberID",
                table: "VisitsPhotos",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitsPhotos_SN",
                table: "VisitsPhotos",
                column: "SN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddNewAnimes");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "VisitsDetails");

            migrationBuilder.DropTable(
                name: "VisitsPhotos");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "VisitsTags");

            migrationBuilder.DropTable(
                name: "Visits");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "MemberRoles");
        }
    }
}
