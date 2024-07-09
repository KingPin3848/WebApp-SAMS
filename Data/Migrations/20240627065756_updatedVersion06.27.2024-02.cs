using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0627202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HandheldScannerNodeModels",
                columns: table => new
                {
                    ScannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomIDMod = table.Column<int>(type: "int", nullable: false),
                    SerialNumberMod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandheldScannerNodeModels", x => x.ScannerID);
                    table.ForeignKey(
                        name: "FK_HandheldScannerNodeModels_RoomLocationInfoModels_RoomIDMod",
                        column: x => x.RoomIDMod,
                        principalTable: "RoomLocationInfoModels",
                        principalColumn: "RoomNumberMod");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HandheldScannerNodeModels_RoomIDMod",
                table: "HandheldScannerNodeModels",
                column: "RoomIDMod",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HandheldScannerNodeModels");
        }
    }
}
