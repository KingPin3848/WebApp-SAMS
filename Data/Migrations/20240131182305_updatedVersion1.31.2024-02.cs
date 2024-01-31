using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion131202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels");

            migrationBuilder.AddForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels");

            migrationBuilder.AddForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
