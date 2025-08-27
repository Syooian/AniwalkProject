using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniwalkServer.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteDateForCommentsAndReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Replies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "Comments");
        }
    }
}
