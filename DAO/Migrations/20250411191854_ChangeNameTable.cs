using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloorUser");

            migrationBuilder.CreateTable(
                name: "PrivateFloorUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", unicode: false, fixedLength: true, maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "char(36)", unicode: false, maxLength: 36, nullable: false),
                    FloorId = table.Column<string>(type: "char(36)", unicode: false, maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateFloorUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateFloorUser_InteractiveFloor_FloorId",
                        column: x => x.FloorId,
                        principalTable: "InteractiveFloor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateFloorUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateFloorUser_FloorId",
                table: "PrivateFloorUser",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateFloorUser_UserId",
                table: "PrivateFloorUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateFloorUser");

            migrationBuilder.CreateTable(
                name: "FloorUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", unicode: false, fixedLength: true, maxLength: 36, nullable: false),
                    FloorId = table.Column<string>(type: "char(36)", unicode: false, maxLength: 36, nullable: false),
                    UserId = table.Column<string>(type: "char(36)", unicode: false, maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloorUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloorUser_InteractiveFloor_FloorId",
                        column: x => x.FloorId,
                        principalTable: "InteractiveFloor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FloorUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloorUser_FloorId",
                table: "FloorUser",
                column: "FloorId");

            migrationBuilder.CreateIndex(
                name: "IX_FloorUser_UserId",
                table: "FloorUser",
                column: "UserId");
        }
    }
}
