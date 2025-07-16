using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMemberRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RoleID",
                table: "Members",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Members_RoleID",
                table: "Members",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MemberRoles_RoleID",
                table: "Members",
                column: "RoleID",
                principalTable: "MemberRoles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberRoles_RoleID",
                table: "Members");

            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropIndex(
                name: "IX_Members_RoleID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Members");
        }
    }
}
