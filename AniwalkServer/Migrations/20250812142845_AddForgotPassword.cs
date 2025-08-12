using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class AddForgotPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForgotPassword",
                columns: table => new
                {
                    SN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VerifyCodeExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifyCode = table.Column<string>(type: "char(5)", maxLength: 5, nullable: false),
                    MemberID = table.Column<string>(type: "char(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPassword", x => x.SN);
                    table.ForeignKey(
                        name: "FK_ForgotPassword_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForgotPassword_MemberID",
                table: "ForgotPassword",
                column: "MemberID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForgotPassword");
        }
    }
}
