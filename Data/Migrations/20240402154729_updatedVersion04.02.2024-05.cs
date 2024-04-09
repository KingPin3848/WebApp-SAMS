using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0402202405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels",
                column: "RoomNumberMod");

            migrationBuilder.CreateIndex(
                name: "IX_fastPassModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels",
                column: "RoomLocationInfoModelRoomNumberMod");

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels",
                column: "RoomLocationInfoModelRoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels",
                column: "RoomLocationInfoModelRoomNumberMod",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels",
                column: "RoomLocationInfoModelRoomNumberMod",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_fastPassModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropIndex(
                name: "IX_activeCourseInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropColumn(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");
        }
    }
}
