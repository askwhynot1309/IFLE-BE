using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlayHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StopAt",
                table: "PlayHistory");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "PlayHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlayHistory",
                type: "char(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PlayHistory_UserId",
                table: "PlayHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayHistory_User_UserId",
                table: "PlayHistory",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayHistory_User_UserId",
                table: "PlayHistory");

            migrationBuilder.DropIndex(
                name: "IX_PlayHistory_UserId",
                table: "PlayHistory");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "PlayHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlayHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "StopAt",
                table: "PlayHistory",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
