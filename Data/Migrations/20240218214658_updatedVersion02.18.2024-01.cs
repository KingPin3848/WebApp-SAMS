using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0218202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_studentScheduleInfoModels_ScheduleId",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_studentScheduleInfoModels_CourseIDFromStudentSchedule",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomLocationInfoModels_teacherInfoModels_RoomAssignedToTeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomScheduleModels_TeachingScheduleModel_ScheduleID",
                table: "roomScheduleModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomScheduleModels_roomLocationInfoModels_RoomId",
                table: "roomScheduleModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomScheduleModels_teacherInfoModels_TeacherID",
                table: "roomScheduleModels");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingScheduleModel_teacherInfoModels_TeacherID",
                table: "TeachingScheduleModel");

            migrationBuilder.DropTable(
                name: "courseEnrollmentModels");

            migrationBuilder.DropTable(
                name: "studentScheduleInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_roomLocationInfoModels_RoomAssignedToTeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomScheduleModels",
                table: "roomScheduleModels");

            migrationBuilder.DropColumn(
                name: "TeachingScheduleID",
                table: "teacherInfoModels");

            migrationBuilder.DropColumn(
                name: "RoomAssignedToTeacherID",
                table: "roomLocationInfoModels");

            migrationBuilder.DropColumn(
                name: "RoomCodeMod",
                table: "roomLocationInfoModels");

            migrationBuilder.RenameTable(
                name: "roomScheduleModels",
                newName: "RoomScheduleModel");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "bellAttendanceModels",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_ScheduleId",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_roomScheduleModels_TeacherID",
                table: "RoomScheduleModel",
                newName: "IX_RoomScheduleModel_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_roomScheduleModels_ScheduleID",
                table: "RoomScheduleModel",
                newName: "IX_RoomScheduleModel_ScheduleID");

            migrationBuilder.RenameIndex(
                name: "IX_roomScheduleModels_RoomId",
                table: "RoomScheduleModel",
                newName: "IX_RoomScheduleModel_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "RoomAssignedId",
                table: "teacherInfoModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChosenBellSchedule",
                table: "dailyAttendanceModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "BellNumId",
                table: "bellAttendanceModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ChosenBellSchedule",
                table: "bellAttendanceModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "B2BAttChecked",
                table: "activeCourseInfoModels",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DailyAttChecked",
                table: "activeCourseInfoModels",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomScheduleModel",
                table: "RoomScheduleModel",
                column: "RoomScheduleID");

            migrationBuilder.CreateTable(
                name: "schedulerModels",
                columns: table => new
                {
                    NameOfEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "sem1StudSchedules",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Bell1CourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell2MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell2TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell3MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell3TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell4MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell4TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell5MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell5TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell6MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell6TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell7MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell7TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    AvesBellCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    LunchCodeMod = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sem1StudSchedules", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_sem1StudSchedules_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "sem2StudSchedules",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Bell1CourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell2MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell2TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell3MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell3TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell4MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell4TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell5MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell5TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell6MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell6TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell7MonWedCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    Bell7TueThurCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    AvesBellCourseIDMod = table.Column<int>(type: "int", nullable: false),
                    LunchCodeMod = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sem2StudSchedules", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_sem2StudSchedules_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "studentLocationModels",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentLocationModels", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "timestampModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionMade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MadeBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timestampModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                unique: true,
                filter: "[RoomAssignedId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_bellAttendanceModels_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                column: "Sem1StudScheduleStudentID");

            migrationBuilder.CreateIndex(
                name: "IX_bellAttendanceModels_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                column: "Sem2StudScheduleStudentID");

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
                name: "FK_fastPassModels_sem1StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels",
                column: "CourseIDFromStudentSchedule",
                principalTable: "sem1StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_sem2StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels",
                column: "CourseIDFromStudentSchedule",
                principalTable: "sem2StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomScheduleModel_TeachingScheduleModel_ScheduleID",
                table: "RoomScheduleModel",
                column: "ScheduleID",
                principalTable: "TeachingScheduleModel",
                principalColumn: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomScheduleModel_roomLocationInfoModels_RoomId",
                table: "RoomScheduleModel",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomScheduleModel_teacherInfoModels_TeacherID",
                table: "RoomScheduleModel",
                column: "TeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingScheduleModel_teacherInfoModels_TeacherID",
                table: "TeachingScheduleModel",
                column: "TeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "FK_fastPassModels_sem1StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_sem2StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomScheduleModel_TeachingScheduleModel_ScheduleID",
                table: "RoomScheduleModel");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomScheduleModel_roomLocationInfoModels_RoomId",
                table: "RoomScheduleModel");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomScheduleModel_teacherInfoModels_TeacherID",
                table: "RoomScheduleModel");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingScheduleModel_teacherInfoModels_TeacherID",
                table: "TeachingScheduleModel");

            migrationBuilder.DropTable(
                name: "schedulerModels");

            migrationBuilder.DropTable(
                name: "sem1StudSchedules");

            migrationBuilder.DropTable(
                name: "sem2StudSchedules");

            migrationBuilder.DropTable(
                name: "studentLocationModels");

            migrationBuilder.DropTable(
                name: "timestampModels");

            migrationBuilder.DropIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_bellAttendanceModels_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropIndex(
                name: "IX_bellAttendanceModels_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomScheduleModel",
                table: "RoomScheduleModel");

            migrationBuilder.DropColumn(
                name: "RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropColumn(
                name: "ChosenBellSchedule",
                table: "dailyAttendanceModels");

            migrationBuilder.DropColumn(
                name: "ChosenBellSchedule",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "Sem1StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "Sem2StudScheduleStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "B2BAttChecked",
                table: "activeCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "DailyAttChecked",
                table: "activeCourseInfoModels");

            migrationBuilder.RenameTable(
                name: "RoomScheduleModel",
                newName: "roomScheduleModels");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "bellAttendanceModels",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_CourseId",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomScheduleModel_TeacherID",
                table: "roomScheduleModels",
                newName: "IX_roomScheduleModels_TeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_RoomScheduleModel_ScheduleID",
                table: "roomScheduleModels",
                newName: "IX_roomScheduleModels_ScheduleID");

            migrationBuilder.RenameIndex(
                name: "IX_RoomScheduleModel_RoomId",
                table: "roomScheduleModels",
                newName: "IX_roomScheduleModels_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "TeachingScheduleID",
                table: "teacherInfoModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoomAssignedToTeacherID",
                table: "roomLocationInfoModels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoomCodeMod",
                table: "roomLocationInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "BellNumId",
                table: "bellAttendanceModels",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomScheduleModels",
                table: "roomScheduleModels",
                column: "RoomScheduleID");

            migrationBuilder.CreateTable(
                name: "studentScheduleInfoModels",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AvesBellRoomCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell1EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell2EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell3EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell4EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell5EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell6EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell7EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    LunchCodeMod = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentScheduleInfoModels", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_studentScheduleInfoModels_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "courseEnrollmentModels",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentCourseId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentStudentId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courseEnrollmentModels", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_courseEnrollmentModels_activeCourseInfoModels_EnrollmentCourseId",
                        column: x => x.EnrollmentCourseId,
                        principalTable: "activeCourseInfoModels",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_courseEnrollmentModels_studentInfoModels_EnrollmentStudentId",
                        column: x => x.EnrollmentStudentId,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_courseEnrollmentModels_studentScheduleInfoModels_EnrollmentStudentId",
                        column: x => x.EnrollmentStudentId,
                        principalTable: "studentScheduleInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_RoomAssignedToTeacherID",
                table: "roomLocationInfoModels",
                column: "RoomAssignedToTeacherID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_courseEnrollmentModels_EnrollmentCourseId",
                table: "courseEnrollmentModels",
                column: "EnrollmentCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_courseEnrollmentModels_EnrollmentStudentId",
                table: "courseEnrollmentModels",
                column: "EnrollmentStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_bellAttendanceModels_studentScheduleInfoModels_ScheduleId",
                table: "bellAttendanceModels",
                column: "ScheduleId",
                principalTable: "studentScheduleInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_studentScheduleInfoModels_CourseIDFromStudentSchedule",
                table: "fastPassModels",
                column: "CourseIDFromStudentSchedule",
                principalTable: "studentScheduleInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomLocationInfoModels_teacherInfoModels_RoomAssignedToTeacherID",
                table: "roomLocationInfoModels",
                column: "RoomAssignedToTeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomScheduleModels_TeachingScheduleModel_ScheduleID",
                table: "roomScheduleModels",
                column: "ScheduleID",
                principalTable: "TeachingScheduleModel",
                principalColumn: "ScheduleID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomScheduleModels_roomLocationInfoModels_RoomId",
                table: "roomScheduleModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_roomScheduleModels_teacherInfoModels_TeacherID",
                table: "roomScheduleModels",
                column: "TeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingScheduleModel_teacherInfoModels_TeacherID",
                table: "TeachingScheduleModel",
                column: "TeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");
        }
    }
}
