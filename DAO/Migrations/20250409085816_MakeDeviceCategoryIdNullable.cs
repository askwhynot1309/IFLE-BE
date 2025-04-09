using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class MakeDeviceCategoryIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Device",
                newName: "Uri");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCategoryId",
                table: "Device",
                type: "char(36)",
                unicode: false,
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldUnicode: false,
                oldMaxLength: 36);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Device",
                newName: "SerialNumber");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceCategoryId",
                table: "Device",
                type: "char(36)",
                unicode: false,
                maxLength: 36,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldUnicode: false,
                oldMaxLength: 36,
                oldNullable: true);
        }
    }
}
