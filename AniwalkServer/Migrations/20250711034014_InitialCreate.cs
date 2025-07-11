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
                name: "AnimesCountries",
                columns: table => new
                {
                    AnimesAnimeID = table.Column<string>(type: "char(4)", nullable: false),
                    CountriesCountryCode = table.Column<string>(type: "char(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimesCountries", x => new { x.AnimesAnimeID, x.CountriesCountryCode });
                    table.ForeignKey(
                        name: "FK_AnimesCountries_Animes_AnimesAnimeID",
                        column: x => x.AnimesAnimeID,
                        principalTable: "Animes",
                        principalColumn: "AnimeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimesCountries_Countries_CountriesCountryCode",
                        column: x => x.CountriesCountryCode,
                        principalTable: "Countries",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimesCountries_CountriesCountryCode",
                table: "AnimesCountries",
                column: "CountriesCountryCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimesCountries");

            migrationBuilder.DropTable(
                name: "Animes");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
