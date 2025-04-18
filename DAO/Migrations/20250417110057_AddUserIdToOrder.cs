using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserPackageOrder",
                type: "char(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GamePackageOrder",
                type: "char(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackageOrder_UserId",
                table: "UserPackageOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePackageOrder_UserId",
                table: "GamePackageOrder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePackageOrder_User_UserId",
                table: "GamePackageOrder",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPackageOrder_User_UserId",
                table: "UserPackageOrder",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePackageOrder_User_UserId",
                table: "GamePackageOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPackageOrder_User_UserId",
                table: "UserPackageOrder");

            migrationBuilder.DropIndex(
                name: "IX_UserPackageOrder_UserId",
                table: "UserPackageOrder");

            migrationBuilder.DropIndex(
                name: "IX_GamePackageOrder_UserId",
                table: "GamePackageOrder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPackageOrder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GamePackageOrder");
        }
    }
}
