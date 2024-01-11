using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion110202402 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId1",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID",
                table: "hallPassInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID1",
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
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_AdminInfoModelAdminID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_AdminInfoModelAdminID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_CounselorModelCounselorId",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_CounselorModelCounselorId1",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_NurseInfoModelNurseID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_NurseInfoModelNurseID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminInfoModelAdminID",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminInfoModelAdminID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "CounselorModelCounselorId",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "CounselorModelCounselorId1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "NurseInfoModelNurseID",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "NurseInfoModelNurseID1",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "TeacherInfoModelTeacherID",
                table: "hallPassInfoModels");

            migrationBuilder.DropColumn(
                name: "TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels");

            migrationBuilder.AlterColumn<string>(
                name: "HallPassAssignedByID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "HallPassAddressedByID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels",
                column: "HallPassAddressedByID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels",
                column: "HallPassAssignedByID");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_HallPassAddressedByID",
                table: "hallPassInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_hallPassInfoModels_HallPassAssignedByID",
                table: "hallPassInfoModels");

            migrationBuilder.AlterColumn<string>(
                name: "HallPassAssignedByID",
                table: "hallPassInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "HallPassAddressedByID",
                table: "hallPassInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AdminInfoModelAdminID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminInfoModelAdminID1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounselorModelCounselorId",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounselorModelCounselorId1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NurseInfoModelNurseID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NurseInfoModelNurseID1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherInfoModelTeacherID",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_AdminInfoModelAdminID",
                table: "hallPassInfoModels",
                column: "AdminInfoModelAdminID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_AdminInfoModelAdminID1",
                table: "hallPassInfoModels",
                column: "AdminInfoModelAdminID1");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels",
                column: "AttendanceOfficeMemberModelAoMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels",
                column: "AttendanceOfficeMemberModelAoMemberID1");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_CounselorModelCounselorId",
                table: "hallPassInfoModels",
                column: "CounselorModelCounselorId");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_CounselorModelCounselorId1",
                table: "hallPassInfoModels",
                column: "CounselorModelCounselorId1");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels",
                column: "LawEnforcementInfoModelLawenfID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels",
                column: "LawEnforcementInfoModelLawenfID1");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_NurseInfoModelNurseID",
                table: "hallPassInfoModels",
                column: "NurseInfoModelNurseID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_NurseInfoModelNurseID1",
                table: "hallPassInfoModels",
                column: "NurseInfoModelNurseID1");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID1");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID",
                table: "hallPassInfoModels",
                column: "AdminInfoModelAdminID",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID1",
                table: "hallPassInfoModels",
                column: "AdminInfoModelAdminID1",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID",
                table: "hallPassInfoModels",
                column: "AttendanceOfficeMemberModelAoMemberID",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID1",
                table: "hallPassInfoModels",
                column: "AttendanceOfficeMemberModelAoMemberID1",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId",
                table: "hallPassInfoModels",
                column: "CounselorModelCounselorId",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId1",
                table: "hallPassInfoModels",
                column: "CounselorModelCounselorId1",
                principalTable: "counselorModels",
                principalColumn: "CounselorId");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID",
                table: "hallPassInfoModels",
                column: "LawEnforcementInfoModelLawenfID",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID1",
                table: "hallPassInfoModels",
                column: "LawEnforcementInfoModelLawenfID1",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID",
                table: "hallPassInfoModels",
                column: "NurseInfoModelNurseID",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID1",
                table: "hallPassInfoModels",
                column: "NurseInfoModelNurseID1",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID1",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_adminInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "adminInfoModels",
                principalColumn: "AdminID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "attendanceOfficeMemberModels",
                principalColumn: "AoMemberID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "counselorModels",
                principalColumn: "CounselorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_counselorModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "counselorModels",
                principalColumn: "CounselorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "lawEnforcementInfoModels",
                principalColumn: "LawenfID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "nurseInfoModels",
                principalColumn: "NurseID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy",
                principalTable: "teacherInfoModels",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
