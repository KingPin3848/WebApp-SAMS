using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion22202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels");

            migrationBuilder.DropIndex(
                name: "IX_roomQRCodeModels_RoomId",
                table: "roomQRCodeModels");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "roomQRCodeModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "roomQRCodeModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_roomQRCodeModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                unique: true);
        }
    }
}
