using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0423202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_activeCourseInfoModels_CourseId",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_sem1StudSchedules_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_sem2StudSchedules_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_studentInfoModels_StudentId",
                table: "bellAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels");

            migrationBuilder.DropIndex(
                name: "IX_bellAttendanceModels_CourseId",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "StudentIdMod",
                table: "bellAttendanceModels");

            migrationBuilder.RenameColumn(
                name: "Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "StudentInfoStudentID");

            migrationBuilder.RenameColumn(
                name: "Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "ActiveCoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_StudentInfoStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_ActiveCoursesCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_activeCourseInfoModels_ActiveCoursesCourseId",
                table: "bellAttendanceModels",
                column: "ActiveCoursesCourseId",
                principalTable: "activeCourseInfoModels",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_studentInfoModels_StudentInfoStudentID",
                table: "bellAttendanceModels",
                column: "StudentInfoStudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_activeCourseInfoModels_ActiveCoursesCourseId",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_studentInfoModels_StudentInfoStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.RenameColumn(
                name: "StudentInfoStudentID",
                table: "bellAttendanceModels",
                newName: "Sem2StudScheduleStudentID");

            migrationBuilder.RenameColumn(
                name: "ActiveCoursesCourseId",
                table: "bellAttendanceModels",
                newName: "Sem1StudScheduleStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_StudentInfoStudentID",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_Sem2StudScheduleStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_ActiveCoursesCourseId",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_Sem1StudScheduleStudentID");

            migrationBuilder.AddColumn<int>(
                name: "StudentIdMod",
                table: "bellAttendanceModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels",
                column: "StudentIdMod");

            migrationBuilder.CreateIndex(
                name: "IX_bellAttendanceModels_CourseId",
                table: "bellAttendanceModels",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_activeCourseInfoModels_CourseId",
                table: "bellAttendanceModels",
                column: "CourseId",
                principalTable: "activeCourseInfoModels",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_sem1StudSchedules_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                column: "Sem1StudScheduleStudentID",
                principalTable: "sem1StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_sem2StudSchedules_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                column: "Sem2StudScheduleStudentID",
                principalTable: "sem2StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_studentInfoModels_StudentId",
                table: "bellAttendanceModels",
                column: "StudentIdMod",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");
        }
    }
}
