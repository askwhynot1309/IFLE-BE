using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceCategoryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceInfoUrl",
                table: "DeviceCategory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HFov",
                table: "DeviceCategory",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VFov",
                table: "DeviceCategory",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceInfoUrl",
                table: "DeviceCategory");

            migrationBuilder.DropColumn(
                name: "HFov",
                table: "DeviceCategory");

            migrationBuilder.DropColumn(
                name: "VFov",
                table: "DeviceCategory");
        }
    }
}
