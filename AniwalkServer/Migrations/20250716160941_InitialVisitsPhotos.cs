using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialVisitsPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "VisitsPhotos");
        }
    }
}
