using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActivated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "GamePackageOrder");

            migrationBuilder.DropColumn(
                name: "ActivationKey",
                table: "GamePackageOrder");

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "GamePackageOrder",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "GamePackageOrder");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationDate",
                table: "GamePackageOrder",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivationKey",
                table: "GamePackageOrder",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
