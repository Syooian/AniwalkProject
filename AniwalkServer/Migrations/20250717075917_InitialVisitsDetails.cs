using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialVisitsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitsVisitsTags");

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

            migrationBuilder.CreateIndex(
                name: "IX_VisitsDetails_VisitSN",
                table: "VisitsDetails",
                column: "VisitSN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitsDetails");

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
                name: "IX_VisitsVisitsTags_VisitsTagSN",
                table: "VisitsVisitsTags",
                column: "VisitsTagSN");
        }
    }
}
