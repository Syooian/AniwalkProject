using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class AddMember_Visit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SN",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SN",
                table: "Comments",
                column: "SN");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Visits_SN",
                table: "Comments",
                column: "SN",
                principalTable: "Visits",
                principalColumn: "SN",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Visits_SN",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SN",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SN",
                table: "Comments");
        }
    }
}
