using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0627202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_CourseRoomID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_teacherInfoModels_CourseTeacherID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_activeCourseInfoModels_ActiveCoursesCourseId",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_bellAttendanceModels_studentInfoModels_StudentInfoStudentID",
                table: "bellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_dailyAttendanceModels_studentInfoModels_StudentId",
                table: "dailyAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_sem1StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_sem2StudSchedules_CourseIDFromStudentSchedule",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_fastPassModels_studentInfoModels_StudentID",
                table: "fastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_studentInfoModels_StudentID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_studentInfoModels_StudentID",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels");

            migrationBuilder.DropForeignKey(
                name: "FK_sem1StudSchedules_studentInfoModels_StudentID",
                table: "sem1StudSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_sem2StudSchedules_studentInfoModels_StudentID",
                table: "sem2StudSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_counselorModels_StudentCounselorID",
                table: "studentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_eASuportInfoModels_EASuportEaID",
                table: "studentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_eASuportInfoModels_StudentEAID",
                table: "studentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels");

            migrationBuilder.DropTable(
                name: "synnLabQRNodeModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_twoHrDelayBellScheduleModels",
                table: "twoHrDelayBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timestampModels",
                table: "timestampModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teacherInfoModels",
                table: "teacherInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_studentLocationModels",
                table: "studentLocationModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_studentInfoModels",
                table: "studentInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sem2StudSchedules",
                table: "sem2StudSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sem1StudSchedules",
                table: "sem1StudSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedulerModels",
                table: "schedulerModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pepRallyBellScheduleModels",
                table: "pepRallyBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_passRequestInfoModels",
                table: "passRequestInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_nurseInfoModels",
                table: "nurseInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lawEnforcementInfoModels",
                table: "lawEnforcementInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hallPassInfoModels",
                table: "hallPassInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fastPassModels",
                table: "fastPassModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_extendedAvesModels",
                table: "extendedAvesModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eASuportInfoModels",
                table: "eASuportInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_developerInfoModels",
                table: "developerInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dailyBellScheduleModels",
                table: "dailyBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dailyAttendanceModels",
                table: "dailyAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_counselorModels",
                table: "counselorModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chosenBellSchedModels",
                table: "chosenBellSchedModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_attendanceOfficeMemberModels",
                table: "attendanceOfficeMemberModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_adminInfoModels",
                table: "adminInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activeCourseInfoModels",
                table: "activeCourseInfoModels");

            migrationBuilder.RenameTable(
                name: "twoHrDelayBellScheduleModels",
                newName: "TwoHrDelayBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "timestampModels",
                newName: "TimestampModels");

            migrationBuilder.RenameTable(
                name: "teacherInfoModels",
                newName: "TeacherInfoModels");

            migrationBuilder.RenameTable(
                name: "studentLocationModels",
                newName: "StudentLocationModels");

            migrationBuilder.RenameTable(
                name: "studentInfoModels",
                newName: "StudentInfoModels");

            migrationBuilder.RenameTable(
                name: "sem2StudSchedules",
                newName: "Sem2StudSchedules");

            migrationBuilder.RenameTable(
                name: "sem1StudSchedules",
                newName: "Sem1StudSchedules");

            migrationBuilder.RenameTable(
                name: "schedulerModels",
                newName: "SchedulerModels");

            migrationBuilder.RenameTable(
                name: "roomQRCodeModels",
                newName: "RoomQRCodeModels");

            migrationBuilder.RenameTable(
                name: "roomLocationInfoModels",
                newName: "RoomLocationInfoModels");

            migrationBuilder.RenameTable(
                name: "pepRallyBellScheduleModels",
                newName: "PepRallyBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "passRequestInfoModels",
                newName: "PassRequestInfoModels");

            migrationBuilder.RenameTable(
                name: "nurseInfoModels",
                newName: "NurseInfoModels");

            migrationBuilder.RenameTable(
                name: "lawEnforcementInfoModels",
                newName: "LawEnforcementInfoModels");

            migrationBuilder.RenameTable(
                name: "hallPassInfoModels",
                newName: "HallPassInfoModels");

            migrationBuilder.RenameTable(
                name: "fastPassModels",
                newName: "FastPassModels");

            migrationBuilder.RenameTable(
                name: "extendedAvesModels",
                newName: "ExtendedAvesModels");

            migrationBuilder.RenameTable(
                name: "eASuportInfoModels",
                newName: "EASuportInfoModels");

            migrationBuilder.RenameTable(
                name: "developerInfoModels",
                newName: "DeveloperInfoModels");

            migrationBuilder.RenameTable(
                name: "dailyBellScheduleModels",
                newName: "DailyBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "dailyAttendanceModels",
                newName: "DailyAttendanceModels");

            migrationBuilder.RenameTable(
                name: "counselorModels",
                newName: "CounselorModels");

            migrationBuilder.RenameTable(
                name: "chosenBellSchedModels",
                newName: "ChosenBellSchedModels");

            migrationBuilder.RenameTable(
                name: "bellAttendanceModels",
                newName: "BellAttendanceModels");

            migrationBuilder.RenameTable(
                name: "attendanceOfficeMemberModels",
                newName: "AttendanceOfficeMemberModels");

            migrationBuilder.RenameTable(
                name: "adminInfoModels",
                newName: "AdminInfoModels");

            migrationBuilder.RenameTable(
                name: "activeCourseInfoModels",
                newName: "ActiveCourseInfoModels");

            migrationBuilder.RenameIndex(
                name: "IX_teacherInfoModels_RoomAssignedId",
                table: "TeacherInfoModels",
                newName: "IX_TeacherInfoModels_RoomAssignedId");

            migrationBuilder.RenameIndex(
                name: "IX_studentInfoModels_StudentEAID",
                table: "StudentInfoModels",
                newName: "IX_StudentInfoModels_StudentEAID");

            migrationBuilder.RenameIndex(
                name: "IX_studentInfoModels_StudentCounselorID",
                table: "StudentInfoModels",
                newName: "IX_StudentInfoModels_StudentCounselorID");

            migrationBuilder.RenameIndex(
                name: "IX_studentInfoModels_EASuportEaID",
                table: "StudentInfoModels",
                newName: "IX_StudentInfoModels_EASuportEaID");

            migrationBuilder.RenameIndex(
                name: "IX_passRequestInfoModels_StudentID",
                table: "PassRequestInfoModels",
                newName: "IX_PassRequestInfoModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_passRequestInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                newName: "IX_PassRequestInfoModels_HallPassAssignedBy");

            migrationBuilder.RenameIndex(
                name: "IX_passRequestInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                newName: "IX_PassRequestInfoModels_HallPassAddressedBy");

            migrationBuilder.RenameIndex(
                name: "IX_hallPassInfoModels_StudentID",
                table: "HallPassInfoModels",
                newName: "IX_HallPassInfoModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_hallPassInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                newName: "IX_HallPassInfoModels_HallPassAssignedByID");

            migrationBuilder.RenameIndex(
                name: "IX_hallPassInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                newName: "IX_HallPassInfoModels_HallPassAddressedByID");

            migrationBuilder.RenameIndex(
                name: "IX_fastPassModels_StudentID",
                table: "FastPassModels",
                newName: "IX_FastPassModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_fastPassModels_EndLocationID",
                table: "FastPassModels",
                newName: "IX_FastPassModels_EndLocationID");

            migrationBuilder.RenameIndex(
                name: "IX_fastPassModels_CourseIDFromStudentSchedule",
                table: "FastPassModels",
                newName: "IX_FastPassModels_CourseIDFromStudentSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_dailyAttendanceModels_StudentId",
                table: "DailyAttendanceModels",
                newName: "IX_DailyAttendanceModels_StudentId");

            migrationBuilder.RenameColumn(
                name: "StudentInfoStudentID",
                table: "BellAttendanceModels",
                newName: "Sem2StudScheduleStudentID");

            migrationBuilder.RenameColumn(
                name: "ActiveCoursesCourseId",
                table: "BellAttendanceModels",
                newName: "Sem1StudScheduleStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_StudentInfoStudentID",
                table: "BellAttendanceModels",
                newName: "IX_BellAttendanceModels_Sem2StudScheduleStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_bellAttendanceModels_ActiveCoursesCourseId",
                table: "BellAttendanceModels",
                newName: "IX_BellAttendanceModels_Sem1StudScheduleStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_activeCourseInfoModels_CourseTeacherID",
                table: "ActiveCourseInfoModels",
                newName: "IX_ActiveCourseInfoModels_CourseTeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_activeCourseInfoModels_CourseRoomID",
                table: "ActiveCourseInfoModels",
                newName: "IX_ActiveCourseInfoModels_CourseRoomID");

            migrationBuilder.AddColumn<int>(
                name: "StudentPin",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TwoHrDelayBellScheduleModels",
                table: "TwoHrDelayBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimestampModels",
                table: "TimestampModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherInfoModels",
                table: "TeacherInfoModels",
                column: "TeacherID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentLocationModels",
                table: "StudentLocationModels",
                column: "StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentInfoModels",
                table: "StudentInfoModels",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sem2StudSchedules",
                table: "Sem2StudSchedules",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sem1StudSchedules",
                table: "Sem1StudSchedules",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchedulerModels",
                table: "SchedulerModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomQRCodeModels",
                table: "RoomQRCodeModels",
                column: "RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomLocationInfoModels",
                table: "RoomLocationInfoModels",
                column: "RoomNumberMod");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PepRallyBellScheduleModels",
                table: "PepRallyBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassRequestInfoModels",
                table: "PassRequestInfoModels",
                column: "PassRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NurseInfoModels",
                table: "NurseInfoModels",
                column: "NurseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LawEnforcementInfoModels",
                table: "LawEnforcementInfoModels",
                column: "LawenfID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HallPassInfoModels",
                table: "HallPassInfoModels",
                column: "HallPassID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FastPassModels",
                table: "FastPassModels",
                column: "FastPassIDMod");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExtendedAvesModels",
                table: "ExtendedAvesModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EASuportInfoModels",
                table: "EASuportInfoModels",
                column: "EaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeveloperInfoModels",
                table: "DeveloperInfoModels",
                column: "DeveloperID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyBellScheduleModels",
                table: "DailyBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyAttendanceModels",
                table: "DailyAttendanceModels",
                column: "AttendanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounselorModels",
                table: "CounselorModels",
                column: "CounselorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChosenBellSchedModels",
                table: "ChosenBellSchedModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BellAttendanceModels",
                table: "BellAttendanceModels",
                column: "BellAttendanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttendanceOfficeMemberModels",
                table: "AttendanceOfficeMemberModels",
                column: "AoMemberID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminInfoModels",
                table: "AdminInfoModels",
                column: "AdminID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveCourseInfoModels",
                table: "ActiveCourseInfoModels",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_BellAttendanceModels_CourseId",
                table: "BellAttendanceModels",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_BellAttendanceModels_StudentId",
                table: "BellAttendanceModels",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveCourseInfoModels_RoomLocationInfoModels_CourseRoomID",
                table: "ActiveCourseInfoModels",
                column: "CourseRoomID",
                principalTable: "RoomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_ActiveCourseInfoModels_TeacherInfoModels_CourseTeacherID",
                table: "ActiveCourseInfoModels",
                column: "CourseTeacherID",
                principalTable: "TeacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_BellAttendanceModels_ActiveCourseInfoModels_CourseId",
                table: "BellAttendanceModels",
                column: "CourseId",
                principalTable: "ActiveCourseInfoModels",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BellAttendanceModels_Sem1StudSchedules_Sem1StudScheduleStudentID",
                table: "BellAttendanceModels",
                column: "Sem1StudScheduleStudentID",
                principalTable: "Sem1StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_BellAttendanceModels_Sem2StudSchedules_Sem2StudScheduleStudentID",
                table: "BellAttendanceModels",
                column: "Sem2StudScheduleStudentID",
                principalTable: "Sem2StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_BellAttendanceModels_StudentInfoModels_StudentId",
                table: "BellAttendanceModels",
                column: "StudentId",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyAttendanceModels_StudentInfoModels_StudentId",
                table: "DailyAttendanceModels",
                column: "StudentId",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPassModels_RoomLocationInfoModels_EndLocationID",
                table: "FastPassModels",
                column: "EndLocationID",
                principalTable: "RoomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPassModels_Sem1StudSchedules_CourseIDFromStudentSchedule",
                table: "FastPassModels",
                column: "CourseIDFromStudentSchedule",
                principalTable: "Sem1StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPassModels_Sem2StudSchedules_CourseIDFromStudentSchedule",
                table: "FastPassModels",
                column: "CourseIDFromStudentSchedule",
                principalTable: "Sem2StudSchedules",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_FastPassModels_StudentInfoModels_StudentID",
                table: "FastPassModels",
                column: "StudentID",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_AdminInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "AdminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_AdminInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "AdminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_AttendanceOfficeMemberModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "AttendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_AttendanceOfficeMemberModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "AttendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_CounselorModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "CounselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_CounselorModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "CounselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_LawEnforcementInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "LawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_LawEnforcementInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "LawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_NurseInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "NurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_NurseInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "NurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_StudentInfoModels_StudentID",
                table: "HallPassInfoModels",
                column: "StudentID",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_TeacherInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "TeacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_HallPassInfoModels_TeacherInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "TeacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_AdminInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "AdminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_AdminInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "AdminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_AttendanceOfficeMemberModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "AttendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_AttendanceOfficeMemberModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "AttendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_CounselorModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "CounselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_CounselorModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "CounselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_LawEnforcementInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "LawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_LawEnforcementInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "LawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_NurseInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "NurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_NurseInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "NurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_StudentInfoModels_StudentID",
                table: "PassRequestInfoModels",
                column: "StudentID",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_TeacherInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "TeacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_PassRequestInfoModels_TeacherInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "TeacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomQRCodeModels_RoomLocationInfoModels_RoomId",
                table: "RoomQRCodeModels",
                column: "RoomId",
                principalTable: "RoomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_Sem1StudSchedules_StudentInfoModels_StudentID",
                table: "Sem1StudSchedules",
                column: "StudentID",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Sem2StudSchedules_StudentInfoModels_StudentID",
                table: "Sem2StudSchedules",
                column: "StudentID",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfoModels_CounselorModels_StudentCounselorID",
                table: "StudentInfoModels",
                column: "StudentCounselorID",
                principalTable: "CounselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfoModels_EASuportInfoModels_EASuportEaID",
                table: "StudentInfoModels",
                column: "EASuportEaID",
                principalTable: "EASuportInfoModels",
                principalColumn: "EaID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfoModels_EASuportInfoModels_StudentEAID",
                table: "StudentInfoModels",
                column: "StudentEAID",
                principalTable: "EASuportInfoModels",
                principalColumn: "EaID");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherInfoModels_RoomLocationInfoModels_RoomAssignedId",
                table: "TeacherInfoModels",
                column: "RoomAssignedId",
                principalTable: "RoomLocationInfoModels",
                principalColumn: "RoomNumberMod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActiveCourseInfoModels_RoomLocationInfoModels_CourseRoomID",
                table: "ActiveCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_ActiveCourseInfoModels_TeacherInfoModels_CourseTeacherID",
                table: "ActiveCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_BellAttendanceModels_ActiveCourseInfoModels_CourseId",
                table: "BellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_BellAttendanceModels_Sem1StudSchedules_Sem1StudScheduleStudentID",
                table: "BellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_BellAttendanceModels_Sem2StudSchedules_Sem2StudScheduleStudentID",
                table: "BellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_BellAttendanceModels_StudentInfoModels_StudentId",
                table: "BellAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyAttendanceModels_StudentInfoModels_StudentId",
                table: "DailyAttendanceModels");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPassModels_RoomLocationInfoModels_EndLocationID",
                table: "FastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPassModels_Sem1StudSchedules_CourseIDFromStudentSchedule",
                table: "FastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPassModels_Sem2StudSchedules_CourseIDFromStudentSchedule",
                table: "FastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_FastPassModels_StudentInfoModels_StudentID",
                table: "FastPassModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_AdminInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_AdminInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_AttendanceOfficeMemberModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_AttendanceOfficeMemberModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_CounselorModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_CounselorModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_LawEnforcementInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_LawEnforcementInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_NurseInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_NurseInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_StudentInfoModels_StudentID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_TeacherInfoModels_HallPassAddressedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_HallPassInfoModels_TeacherInfoModels_HallPassAssignedByID",
                table: "HallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_AdminInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_AdminInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_AttendanceOfficeMemberModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_AttendanceOfficeMemberModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_CounselorModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_CounselorModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_LawEnforcementInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_LawEnforcementInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_NurseInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_NurseInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_StudentInfoModels_StudentID",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_TeacherInfoModels_HallPassAddressedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_PassRequestInfoModels_TeacherInfoModels_HallPassAssignedBy",
                table: "PassRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomQRCodeModels_RoomLocationInfoModels_RoomId",
                table: "RoomQRCodeModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Sem1StudSchedules_StudentInfoModels_StudentID",
                table: "Sem1StudSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Sem2StudSchedules_StudentInfoModels_StudentID",
                table: "Sem2StudSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfoModels_CounselorModels_StudentCounselorID",
                table: "StudentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfoModels_EASuportInfoModels_EASuportEaID",
                table: "StudentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfoModels_EASuportInfoModels_StudentEAID",
                table: "StudentInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherInfoModels_RoomLocationInfoModels_RoomAssignedId",
                table: "TeacherInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TwoHrDelayBellScheduleModels",
                table: "TwoHrDelayBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimestampModels",
                table: "TimestampModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherInfoModels",
                table: "TeacherInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentLocationModels",
                table: "StudentLocationModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentInfoModels",
                table: "StudentInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sem2StudSchedules",
                table: "Sem2StudSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sem1StudSchedules",
                table: "Sem1StudSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchedulerModels",
                table: "SchedulerModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomQRCodeModels",
                table: "RoomQRCodeModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomLocationInfoModels",
                table: "RoomLocationInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PepRallyBellScheduleModels",
                table: "PepRallyBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassRequestInfoModels",
                table: "PassRequestInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NurseInfoModels",
                table: "NurseInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LawEnforcementInfoModels",
                table: "LawEnforcementInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HallPassInfoModels",
                table: "HallPassInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FastPassModels",
                table: "FastPassModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExtendedAvesModels",
                table: "ExtendedAvesModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EASuportInfoModels",
                table: "EASuportInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeveloperInfoModels",
                table: "DeveloperInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyBellScheduleModels",
                table: "DailyBellScheduleModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyAttendanceModels",
                table: "DailyAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CounselorModels",
                table: "CounselorModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChosenBellSchedModels",
                table: "ChosenBellSchedModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BellAttendanceModels",
                table: "BellAttendanceModels");

            migrationBuilder.DropIndex(
                name: "IX_BellAttendanceModels_CourseId",
                table: "BellAttendanceModels");

            migrationBuilder.DropIndex(
                name: "IX_BellAttendanceModels_StudentId",
                table: "BellAttendanceModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttendanceOfficeMemberModels",
                table: "AttendanceOfficeMemberModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminInfoModels",
                table: "AdminInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveCourseInfoModels",
                table: "ActiveCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "StudentPin",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "TwoHrDelayBellScheduleModels",
                newName: "twoHrDelayBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "TimestampModels",
                newName: "timestampModels");

            migrationBuilder.RenameTable(
                name: "TeacherInfoModels",
                newName: "teacherInfoModels");

            migrationBuilder.RenameTable(
                name: "StudentLocationModels",
                newName: "studentLocationModels");

            migrationBuilder.RenameTable(
                name: "StudentInfoModels",
                newName: "studentInfoModels");

            migrationBuilder.RenameTable(
                name: "Sem2StudSchedules",
                newName: "sem2StudSchedules");

            migrationBuilder.RenameTable(
                name: "Sem1StudSchedules",
                newName: "sem1StudSchedules");

            migrationBuilder.RenameTable(
                name: "SchedulerModels",
                newName: "schedulerModels");

            migrationBuilder.RenameTable(
                name: "RoomQRCodeModels",
                newName: "roomQRCodeModels");

            migrationBuilder.RenameTable(
                name: "RoomLocationInfoModels",
                newName: "roomLocationInfoModels");

            migrationBuilder.RenameTable(
                name: "PepRallyBellScheduleModels",
                newName: "pepRallyBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "PassRequestInfoModels",
                newName: "passRequestInfoModels");

            migrationBuilder.RenameTable(
                name: "NurseInfoModels",
                newName: "nurseInfoModels");

            migrationBuilder.RenameTable(
                name: "LawEnforcementInfoModels",
                newName: "lawEnforcementInfoModels");

            migrationBuilder.RenameTable(
                name: "HallPassInfoModels",
                newName: "hallPassInfoModels");

            migrationBuilder.RenameTable(
                name: "FastPassModels",
                newName: "fastPassModels");

            migrationBuilder.RenameTable(
                name: "ExtendedAvesModels",
                newName: "extendedAvesModels");

            migrationBuilder.RenameTable(
                name: "EASuportInfoModels",
                newName: "eASuportInfoModels");

            migrationBuilder.RenameTable(
                name: "DeveloperInfoModels",
                newName: "developerInfoModels");

            migrationBuilder.RenameTable(
                name: "DailyBellScheduleModels",
                newName: "dailyBellScheduleModels");

            migrationBuilder.RenameTable(
                name: "DailyAttendanceModels",
                newName: "dailyAttendanceModels");

            migrationBuilder.RenameTable(
                name: "CounselorModels",
                newName: "counselorModels");

            migrationBuilder.RenameTable(
                name: "ChosenBellSchedModels",
                newName: "chosenBellSchedModels");

            migrationBuilder.RenameTable(
                name: "BellAttendanceModels",
                newName: "bellAttendanceModels");

            migrationBuilder.RenameTable(
                name: "AttendanceOfficeMemberModels",
                newName: "attendanceOfficeMemberModels");

            migrationBuilder.RenameTable(
                name: "AdminInfoModels",
                newName: "adminInfoModels");

            migrationBuilder.RenameTable(
                name: "ActiveCourseInfoModels",
                newName: "activeCourseInfoModels");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                newName: "IX_teacherInfoModels_RoomAssignedId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentInfoModels_StudentEAID",
                table: "studentInfoModels",
                newName: "IX_studentInfoModels_StudentEAID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentInfoModels_StudentCounselorID",
                table: "studentInfoModels",
                newName: "IX_studentInfoModels_StudentCounselorID");

            migrationBuilder.RenameIndex(
                name: "IX_StudentInfoModels_EASuportEaID",
                table: "studentInfoModels",
                newName: "IX_studentInfoModels_EASuportEaID");

            migrationBuilder.RenameIndex(
                name: "IX_PassRequestInfoModels_StudentID",
                table: "passRequestInfoModels",
                newName: "IX_passRequestInfoModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_PassRequestInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                newName: "IX_passRequestInfoModels_HallPassAssignedBy");

            migrationBuilder.RenameIndex(
                name: "IX_PassRequestInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                newName: "IX_passRequestInfoModels_HallPassAddressedBy");

            migrationBuilder.RenameIndex(
                name: "IX_HallPassInfoModels_StudentID",
                table: "hallPassInfoModels",
                newName: "IX_hallPassInfoModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_HallPassInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                newName: "IX_hallPassInfoModels_HallPassAssignedByID");

            migrationBuilder.RenameIndex(
                name: "IX_HallPassInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                newName: "IX_hallPassInfoModels_HallPassAddressedByID");

            migrationBuilder.RenameIndex(
                name: "IX_FastPassModels_StudentID",
                table: "fastPassModels",
                newName: "IX_fastPassModels_StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_FastPassModels_EndLocationID",
                table: "fastPassModels",
                newName: "IX_fastPassModels_EndLocationID");

            migrationBuilder.RenameIndex(
                name: "IX_FastPassModels_CourseIDFromStudentSchedule",
                table: "fastPassModels",
                newName: "IX_fastPassModels_CourseIDFromStudentSchedule");

            migrationBuilder.RenameIndex(
                name: "IX_DailyAttendanceModels_StudentId",
                table: "dailyAttendanceModels",
                newName: "IX_dailyAttendanceModels_StudentId");

            migrationBuilder.RenameColumn(
                name: "Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "StudentInfoStudentID");

            migrationBuilder.RenameColumn(
                name: "Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "ActiveCoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_BellAttendanceModels_Sem2StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_StudentInfoStudentID");

            migrationBuilder.RenameIndex(
                name: "IX_BellAttendanceModels_Sem1StudScheduleStudentID",
                table: "bellAttendanceModels",
                newName: "IX_bellAttendanceModels_ActiveCoursesCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveCourseInfoModels_CourseTeacherID",
                table: "activeCourseInfoModels",
                newName: "IX_activeCourseInfoModels_CourseTeacherID");

            migrationBuilder.RenameIndex(
                name: "IX_ActiveCourseInfoModels_CourseRoomID",
                table: "activeCourseInfoModels",
                newName: "IX_activeCourseInfoModels_CourseRoomID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_twoHrDelayBellScheduleModels",
                table: "twoHrDelayBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_timestampModels",
                table: "timestampModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teacherInfoModels",
                table: "teacherInfoModels",
                column: "TeacherID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentLocationModels",
                table: "studentLocationModels",
                column: "StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentInfoModels",
                table: "studentInfoModels",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sem2StudSchedules",
                table: "sem2StudSchedules",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sem1StudSchedules",
                table: "sem1StudSchedules",
                column: "StudentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedulerModels",
                table: "schedulerModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomQRCodeModels",
                table: "roomQRCodeModels",
                column: "RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roomLocationInfoModels",
                table: "roomLocationInfoModels",
                column: "RoomNumberMod");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pepRallyBellScheduleModels",
                table: "pepRallyBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_passRequestInfoModels",
                table: "passRequestInfoModels",
                column: "PassRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_nurseInfoModels",
                table: "nurseInfoModels",
                column: "NurseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lawEnforcementInfoModels",
                table: "lawEnforcementInfoModels",
                column: "LawenfID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hallPassInfoModels",
                table: "hallPassInfoModels",
                column: "HallPassID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fastPassModels",
                table: "fastPassModels",
                column: "FastPassIDMod");

            migrationBuilder.AddPrimaryKey(
                name: "PK_extendedAvesModels",
                table: "extendedAvesModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eASuportInfoModels",
                table: "eASuportInfoModels",
                column: "EaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_developerInfoModels",
                table: "developerInfoModels",
                column: "DeveloperID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dailyBellScheduleModels",
                table: "dailyBellScheduleModels",
                column: "StartTime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dailyAttendanceModels",
                table: "dailyAttendanceModels",
                column: "AttendanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_counselorModels",
                table: "counselorModels",
                column: "CounselorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chosenBellSchedModels",
                table: "chosenBellSchedModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels",
                column: "BellAttendanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendanceOfficeMemberModels",
                table: "attendanceOfficeMemberModels",
                column: "AoMemberID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_adminInfoModels",
                table: "adminInfoModels",
                column: "AdminID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activeCourseInfoModels",
                table: "activeCourseInfoModels",
                column: "CourseId");

            migrationBuilder.CreateTable(
                name: "synnLabQRNodeModels",
                columns: table => new
                {
                    ScannerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SynnlabRoomIDMod = table.Column<int>(type: "int", nullable: false),
                    ModelNumberMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScannerDeviceIPAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScannerLabelMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScannerMacAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_synnLabQRNodeModels", x => x.ScannerID);
                    table.ForeignKey(
                        name: "FK_synnLabQRNodeModels_roomLocationInfoModels_SynnlabRoomIDMod",
                        column: x => x.SynnlabRoomIDMod,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomNumberMod");
                });

            migrationBuilder.CreateIndex(
                name: "IX_synnLabQRNodeModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "RoomIDMod",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_activeCourseInfoModels_roomLocationInfoModels_CourseRoomID",
                table: "activeCourseInfoModels",
                column: "CourseRoomID",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_activeCourseInfoModels_teacherInfoModels_CourseTeacherID",
                table: "activeCourseInfoModels",
                column: "CourseTeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_dailyAttendanceModels_studentInfoModels_StudentId",
                table: "dailyAttendanceModels",
                column: "StudentId",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                table: "fastPassModels",
                column: "EndLocationID",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

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
                name: "FK_fastPassModels_studentInfoModels_StudentID",
                table: "fastPassModels",
                column: "StudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_studentInfoModels_StudentID",
                table: "hallPassInfoModels",
                column: "StudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_studentInfoModels_StudentID",
                table: "passRequestInfoModels",
                column: "StudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_roomQRCodeModels_roomLocationInfoModels_RoomId",
                table: "roomQRCodeModels",
                column: "RoomId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");

            migrationBuilder.AddForeignKey(
                name: "FK_sem1StudSchedules_studentInfoModels_StudentID",
                table: "sem1StudSchedules",
                column: "StudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_sem2StudSchedules_studentInfoModels_StudentID",
                table: "sem2StudSchedules",
                column: "StudentID",
                principalTable: "studentInfoModels",
                principalColumn: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_counselorModels_StudentCounselorID",
                table: "studentInfoModels",
                column: "StudentCounselorID",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_eASuportInfoModels_EASuportEaID",
                table: "studentInfoModels",
                column: "EASuportEaID",
                principalTable: "eASuportInfoModels",
                principalColumn: "EaID");

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_eASuportInfoModels_StudentEAID",
                table: "studentInfoModels",
                column: "StudentEAID",
                principalTable: "eASuportInfoModels",
                principalColumn: "EaID");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_roomLocationInfoModels_RoomAssignedId",
                table: "teacherInfoModels",
                column: "RoomAssignedId",
                principalTable: "roomLocationInfoModels",
                principalColumn: "RoomNumberMod");
        }
    }
}
