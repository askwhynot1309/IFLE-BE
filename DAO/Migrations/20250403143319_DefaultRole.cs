using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "9a91761a-afb5-49ac-b42e-bf357e944eab", "Admin" },
                    { "bc60ffc5-e9bb-4c9d-916a-69c673fbb184", "Customer" },
                    { "eade30a8-5e6e-4103-85b2-7e449de61a8b", "Staff" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "9a91761a-afb5-49ac-b42e-bf357e944eab");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "bc60ffc5-e9bb-4c9d-916a-69c673fbb184");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "eade30a8-5e6e-4103-85b2-7e449de61a8b");
        }
    }
}
