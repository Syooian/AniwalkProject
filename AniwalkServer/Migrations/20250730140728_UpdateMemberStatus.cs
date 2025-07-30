using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MemberStatus_StatusCode",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus");

            migrationBuilder.DropIndex(
                name: "IX_Members_StatusCode",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "StatusName",
                table: "MemberStatus");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Members");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "MemberStatus",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberID",
                table: "MemberStatus",
                type: "char(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "MemberStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus",
                column: "MemberID");

            migrationBuilder.CreateTable(
                name: "MemberStatusCode",
                columns: table => new
                {
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatusCode", x => x.StatusCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatus_StatusCode",
                table: "MemberStatus",
                column: "StatusCode");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberStatus_MemberStatusCode_StatusCode",
                table: "MemberStatus",
                column: "StatusCode",
                principalTable: "MemberStatusCode",
                principalColumn: "StatusCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberStatus_Members_MemberID",
                table: "MemberStatus",
                column: "MemberID",
                principalTable: "Members",
                principalColumn: "MemberID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberStatus_MemberStatusCode_StatusCode",
                table: "MemberStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberStatus_Members_MemberID",
                table: "MemberStatus");

            migrationBuilder.DropTable(
                name: "MemberStatusCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus");

            migrationBuilder.DropIndex(
                name: "IX_MemberStatus_StatusCode",
                table: "MemberStatus");

            migrationBuilder.DropColumn(
                name: "MemberID",
                table: "MemberStatus");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "MemberStatus");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "MemberStatus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusName",
                table: "MemberStatus",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberStatus",
                table: "MemberStatus",
                column: "StatusCode");

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
    }
}
