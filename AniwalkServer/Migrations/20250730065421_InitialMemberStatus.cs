using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMemberStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MemberStatus",
                columns: table => new
                {
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_StatusCode",
                table: "Members",
                column: "StatusCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberStatus_StatusCode",
                table: "Members",
                column: "StatusCode",
                principalTable: "MemberStatus",
                principalColumn: "StatusCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberStatus_StatusCode",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberStatus");

            migrationBuilder.DropIndex(
                name: "IX_Members_StatusCode",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Members");
        }
    }
}
