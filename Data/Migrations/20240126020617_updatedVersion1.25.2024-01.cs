using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion125202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentScheduleInfoModels_bellAttendanceModels_StudentID",
                table: "studentScheduleInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_substituteInfoModels_teacherInfoModels_ManagedTeacherIdMod",
                table: "substituteInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_substituteInfoModels_ManagedTeacherIdMod",
                table: "substituteInfoModels");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "bellAttendanceModels");

            migrationBuilder.AddColumn<string>(
                name: "AssignedSubID",
                table: "teacherInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagedTeacherIdMod",
                table: "substituteInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "bellAttendanceModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChosenBellSchedModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChosenBellSchedModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teacherInfoModels_AssignedSubID",
                table: "teacherInfoModels",
                column: "AssignedSubID");

            migrationBuilder.CreateIndex(
                name: "IX_bellAttendanceModels_ScheduleId",
                table: "bellAttendanceModels",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_studentScheduleInfoModels_ScheduleId",
                table: "bellAttendanceModels",
                column: "ScheduleId",
                principalTable: "studentScheduleInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_substituteInfoModels_AssignedSubID",
                table: "teacherInfoModels",
                column: "AssignedSubID",
                principalTable: "substituteInfoModels",
                principalColumn: "SubID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_studentScheduleInfoModels_ScheduleId",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherInfoModels_substituteInfoModels_AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropTable(
                name: "ChosenBellSchedModel");

            migrationBuilder.DropIndex(
                name: "IX_teacherInfoModels_AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_bellAttendanceModels_ScheduleId",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "bellAttendanceModels");

            migrationBuilder.AlterColumn<string>(
                name: "ManagedTeacherIdMod",
                table: "substituteInfoModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "bellAttendanceModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_substituteInfoModels_ManagedTeacherIdMod",
                table: "substituteInfoModels",
                column: "ManagedTeacherIdMod",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_studentScheduleInfoModels_bellAttendanceModels_StudentID",
                table: "studentScheduleInfoModels",
                column: "StudentID",
                principalTable: "bellAttendanceModels",
                principalColumn: "StudentIdMod");

            migrationBuilder.AddForeignKey(
                name: "FK_substituteInfoModels_teacherInfoModels_ManagedTeacherIdMod",
                table: "substituteInfoModels",
                column: "ManagedTeacherIdMod",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");
        }
    }
}
