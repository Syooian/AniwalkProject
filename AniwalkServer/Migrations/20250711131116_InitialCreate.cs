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
                    CountryCode = table.Column<string>(type: "char(3)", nullable: false)
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
                name: "VisitsVisitsTags",
                columns: table => new
                {
                    VisitsSN = table.Column<int>(type: "int", nullable: false),
                    VisitsTagSN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitsVisitsTags", x => new { x.VisitsSN, x.VisitsTagSN });
                    table.ForeignKey(
                        name: "FK_VisitsVisitsTags_VisitsTags_VisitsTagSN",
                        column: x => x.VisitsTagSN,
                        principalTable: "VisitsTags",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VisitsVisitsTags_Visits_VisitsSN",
                        column: x => x.VisitsSN,
                        principalTable: "Visits",
                        principalColumn: "SN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_CountryCode",
                table: "Members",
                column: "CountryCode");

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
                name: "IX_VisitsVisitsTags_VisitsTagSN",
                table: "VisitsVisitsTags",
                column: "VisitsTagSN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitsVisitsTags");

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
        }
    }
}
