using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0408202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomLocationInfoModels_synnLabQRNodeModels_SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomLocationInfoModels_teacherInfoModels_TeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_roomLocationInfoModels_SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_roomLocationInfoModels_TeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_fastPassModels_RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropIndex(
                name: "IX_activeCourseInfoModels_RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropColumn(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels");

            migrationBuilder.DropColumn(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationCode",
                table: "AspNetUsers",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                unique: true,
                filter: "[RoomAssignedId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_synnLabQRNodeModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "RoomIDMod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fastPassModels_EndLocationID",
                table: "fastPassModels",
                column: "EndLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_CourseRoomID",
                table: "activeCourseInfoModels",
                column: "CourseRoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_CourseRoomID",
                table: "activeCourseInfoModels",
                column: "CourseRoomID",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                table: "fastPassModels",
                column: "EndLocationID",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_synnLabQRNodeModels_roomLocationInfoModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "RoomIDMod",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_CourseRoomID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels");

            migrationBuilder.DropForeignKey(
                name: "FK_synnLabQRNodeModels_roomLocationInfoModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_synnLabQRNodeModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels");

            migrationBuilder.DropIndex(
                name: "IX_fastPassModels_EndLocationID",
                table: "fastPassModels");

            migrationBuilder.DropIndex(
                name: "IX_activeCourseInfoModels_CourseRoomID",
                table: "activeCourseInfoModels");

            migrationBuilder.AddColumn<string>(
                name: "SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherID",
                table: "roomLocationInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "fastPassModels",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActivationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<int>(
                name: "RoomLocationInfoModelRoomNumberMod",
                table: "activeCourseInfoModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels",
                column: "SynnLabQRNodeScannerID");

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_TeacherID",
                table: "roomLocationInfoModels",
                column: "TeacherID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_roomLocationInfoModels_synnLabQRNodeModels_SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels",
                column: "SynnLabQRNodeScannerID",
                principalTable: "synnLabQRNodeModels",
                principalColumn: "ScannerID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomLocationInfoModels_teacherInfoModels_TeacherID",
                table: "roomLocationInfoModels",
                column: "TeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");
        }
    }
}
