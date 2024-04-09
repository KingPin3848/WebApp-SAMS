using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0402202404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_fastPassModels_EndLocationID",
                table: "fastPassModels");

            migrationBuilder.DropIndex(
                name: "IX_activeCourseInfoModels_CourseRoomID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "roomLocationInfoModels");

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

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels",
                column: "SynnLabQRNodeScannerID");

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_TeacherID",
                table: "roomLocationInfoModels",
                column: "TeacherID");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "SynnLabQRNodeScannerID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "roomLocationInfoModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                unique: true,
                filter: "[RoomAssignedId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_synnLabQRNodeModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "SynnlabRoomIDMod",
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
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                table: "fastPassModels",
                column: "EndLocationID",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_synnLabQRNodeModels_roomLocationInfoModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "SynnlabRoomIDMod",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");
        }
    }
}
