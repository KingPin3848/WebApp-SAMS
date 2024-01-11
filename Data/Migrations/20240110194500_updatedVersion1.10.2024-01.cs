using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion110202401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activationModels",
                columns: table => new
                {
                    CodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activationModels", x => x.CodeId);
                });

            migrationBuilder.CreateTable(
                name: "adminInfoModels",
                columns: table => new
                {
                    AdminID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminInfoModels", x => x.AdminID);
                });

            migrationBuilder.CreateTable(
                name: "attendanceOfficeMemberModels",
                columns: table => new
                {
                    AoMemberID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AoMemberFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AoMemberMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AoMemberLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AoMemberPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AoMemberEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AoMemberPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendanceOfficeMemberModels", x => x.AoMemberID);
                });

            migrationBuilder.CreateTable(
                name: "counselorModels",
                columns: table => new
                {
                    CounselorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CounselorFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CounselorMiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CounselorLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CounselorPreferredName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CounselorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CounselorPhone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counselorModels", x => x.CounselorId);
                });

            migrationBuilder.CreateTable(
                name: "dailyBellScheduleModels",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyBellScheduleModels", x => x.StartTime);
                });

            migrationBuilder.CreateTable(
                name: "developerInfoModels",
                columns: table => new
                {
                    DeveloperID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeveloperFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeveloperMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeveloperLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeveloperPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeveloperEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developerInfoModels", x => x.DeveloperID);
                });

            migrationBuilder.CreateTable(
                name: "eASuportInfoModels",
                columns: table => new
                {
                    EaID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EaFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EaStudentManaged = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eASuportInfoModels", x => x.EaID);
                });

            migrationBuilder.CreateTable(
                name: "extendedAvesModels",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_extendedAvesModels", x => x.StartTime);
                });

            migrationBuilder.CreateTable(
                name: "lawEnforcementInfoModels",
                columns: table => new
                {
                    LawenfID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LaweFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaweMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaweLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LawePreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaweEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LawePhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lawEnforcementInfoModels", x => x.LawenfID);
                });

            migrationBuilder.CreateTable(
                name: "nurseInfoModels",
                columns: table => new
                {
                    NurseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NurseFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NurseMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NurseLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NursePreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NurseEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NursePhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nurseInfoModels", x => x.NurseID);
                });

            migrationBuilder.CreateTable(
                name: "pepRallyBellScheduleModels",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pepRallyBellScheduleModels", x => x.StartTime);
                });

            migrationBuilder.CreateTable(
                name: "teacherInfoModels",
                columns: table => new
                {
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teaches5Days = table.Column<bool>(type: "bit", nullable: false),
                    TeachingScheduleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherInfoModels", x => x.TeacherID);
                });

            migrationBuilder.CreateTable(
                name: "twoHrDelayBellScheduleModels",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_twoHrDelayBellScheduleModels", x => x.StartTime);
                });

            migrationBuilder.CreateTable(
                name: "studentInfoModels",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentGradYearMod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentCounselorID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasEASupport = table.Column<bool>(type: "bit", nullable: false),
                    StudentEAID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Parentguard1NameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parentguard1EmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parentguard2NameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parentguard2EmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivationCodeId = table.Column<int>(type: "int", nullable: false),
                    EASuportEaID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentInfoModels", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_studentInfoModels_activationModels_ActivationCodeId",
                        column: x => x.ActivationCodeId,
                        principalTable: "activationModels",
                        principalColumn: "CodeId");
                    table.ForeignKey(
                        name: "FK_studentInfoModels_counselorModels_StudentCounselorID",
                        column: x => x.StudentCounselorID,
                        principalTable: "counselorModels",
                        principalColumn: "CounselorId");
                    table.ForeignKey(
                        name: "FK_studentInfoModels_eASuportInfoModels_EASuportEaID",
                        column: x => x.EASuportEaID,
                        principalTable: "eASuportInfoModels",
                        principalColumn: "EaID");
                    table.ForeignKey(
                        name: "FK_studentInfoModels_eASuportInfoModels_StudentEAID",
                        column: x => x.StudentEAID,
                        principalTable: "eASuportInfoModels",
                        principalColumn: "EaID");
                });

            migrationBuilder.CreateTable(
                name: "roomLocationInfoModels",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumberMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WingNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomAssignedToTeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomScannerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomLocationInfoModels", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_roomLocationInfoModels_teacherInfoModels_RoomAssignedToTeacherID",
                        column: x => x.RoomAssignedToTeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateTable(
                name: "substituteInfoModels",
                columns: table => new
                {
                    SubID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagedTeacherIdMod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_substituteInfoModels", x => x.SubID);
                    table.ForeignKey(
                        name: "FK_substituteInfoModels_teacherInfoModels_ManagedTeacherIdMod",
                        column: x => x.ManagedTeacherIdMod,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateTable(
                name: "TeachingScheduleModel",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingScheduleModel", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_TeachingScheduleModel_teacherInfoModels_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateTable(
                name: "bellAttendanceModels",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    BellAttendanceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonForAbsence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BellNumId = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bellAttendanceModels", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_bellAttendanceModels_studentInfoModels_StudentId",
                        column: x => x.StudentId,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "dailyAttendanceModels",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReasonForAbsence = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dailyAttendanceModels", x => x.AttendanceId);
                    table.ForeignKey(
                        name: "FK_dailyAttendanceModels_studentInfoModels_StudentId",
                        column: x => x.StudentId,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "hallPassInfoModels",
                columns: table => new
                {
                    HallPassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellNumber = table.Column<int>(type: "int", nullable: false),
                    HallPassAssignedByID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HallPassAddressedByID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminInfoModelAdminID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdminInfoModelAdminID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AttendanceOfficeMemberModelAoMemberID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AttendanceOfficeMemberModelAoMemberID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CounselorModelCounselorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CounselorModelCounselorId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LawEnforcementInfoModelLawenfID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LawEnforcementInfoModelLawenfID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NurseInfoModelNurseID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NurseInfoModelNurseID1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherInfoModelTeacherID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherInfoModelTeacherID1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hallPassInfoModels", x => x.HallPassID);
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID",
                        column: x => x.AdminInfoModelAdminID,
                        principalTable: "adminInfoModels",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_adminInfoModels_AdminInfoModelAdminID1",
                        column: x => x.AdminInfoModelAdminID1,
                        principalTable: "adminInfoModels",
                        principalColumn: "AdminID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID",
                        column: x => x.AttendanceOfficeMemberModelAoMemberID,
                        principalTable: "attendanceOfficeMemberModels",
                        principalColumn: "AoMemberID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_attendanceOfficeMemberModels_AttendanceOfficeMemberModelAoMemberID1",
                        column: x => x.AttendanceOfficeMemberModelAoMemberID1,
                        principalTable: "attendanceOfficeMemberModels",
                        principalColumn: "AoMemberID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId",
                        column: x => x.CounselorModelCounselorId,
                        principalTable: "counselorModels",
                        principalColumn: "CounselorId");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_counselorModels_CounselorModelCounselorId1",
                        column: x => x.CounselorModelCounselorId1,
                        principalTable: "counselorModels",
                        principalColumn: "CounselorId");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID",
                        column: x => x.LawEnforcementInfoModelLawenfID,
                        principalTable: "lawEnforcementInfoModels",
                        principalColumn: "LawenfID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_lawEnforcementInfoModels_LawEnforcementInfoModelLawenfID1",
                        column: x => x.LawEnforcementInfoModelLawenfID1,
                        principalTable: "lawEnforcementInfoModels",
                        principalColumn: "LawenfID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID",
                        column: x => x.NurseInfoModelNurseID,
                        principalTable: "nurseInfoModels",
                        principalColumn: "NurseID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_nurseInfoModels_NurseInfoModelNurseID1",
                        column: x => x.NurseInfoModelNurseID1,
                        principalTable: "nurseInfoModels",
                        principalColumn: "NurseID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID",
                        column: x => x.TeacherInfoModelTeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                    table.ForeignKey(
                        name: "FK_hallPassInfoModels_teacherInfoModels_TeacherInfoModelTeacherID1",
                        column: x => x.TeacherInfoModelTeacherID1,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateTable(
                name: "passRequestInfoModels",
                columns: table => new
                {
                    PassRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    StudentNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HallPassAssignedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HallPassAddressedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellNumber = table.Column<int>(type: "int", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passRequestInfoModels", x => x.PassRequestId);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_adminInfoModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "adminInfoModels",
                        principalColumn: "AdminID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_adminInfoModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "adminInfoModels",
                        principalColumn: "AdminID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "attendanceOfficeMemberModels",
                        principalColumn: "AoMemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_attendanceOfficeMemberModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "attendanceOfficeMemberModels",
                        principalColumn: "AoMemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_counselorModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "counselorModels",
                        principalColumn: "CounselorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_counselorModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "counselorModels",
                        principalColumn: "CounselorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "lawEnforcementInfoModels",
                        principalColumn: "LawenfID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_lawEnforcementInfoModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "lawEnforcementInfoModels",
                        principalColumn: "LawenfID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "nurseInfoModels",
                        principalColumn: "NurseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_nurseInfoModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "nurseInfoModels",
                        principalColumn: "NurseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAddressedBy",
                        column: x => x.HallPassAddressedBy,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passRequestInfoModels_teacherInfoModels_HallPassAssignedBy",
                        column: x => x.HallPassAssignedBy,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "synnLabQRNodeModels",
                columns: table => new
                {
                    ScannerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SynnlabRoomIDMod = table.Column<int>(type: "int", nullable: false),
                    ScannerMacAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumberMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScannerDeviceIPAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScannerLabelMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_synnLabQRNodeModels", x => x.ScannerID);
                    table.ForeignKey(
                        name: "FK_synnLabQRNodeModels_roomLocationInfoModels_SynnlabRoomIDMod",
                        column: x => x.SynnlabRoomIDMod,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomId");
                });

            migrationBuilder.CreateTable(
                name: "activeCourseInfoModels",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseTeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseRoomID = table.Column<int>(type: "int", nullable: false),
                    CourseBellNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseLength = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubstituteSubID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activeCourseInfoModels", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_activeCourseInfoModels_roomLocationInfoModels_CourseRoomID",
                        column: x => x.CourseRoomID,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_activeCourseInfoModels_substituteInfoModels_SubstituteSubID",
                        column: x => x.SubstituteSubID,
                        principalTable: "substituteInfoModels",
                        principalColumn: "SubID");
                    table.ForeignKey(
                        name: "FK_activeCourseInfoModels_teacherInfoModels_CourseTeacherID",
                        column: x => x.CourseTeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateTable(
                name: "roomScheduleModels",
                columns: table => new
                {
                    RoomScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomScheduleModels", x => x.RoomScheduleID);
                    table.ForeignKey(
                        name: "FK_roomScheduleModels_roomLocationInfoModels_RoomId",
                        column: x => x.RoomId,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_roomScheduleModels_teacherInfoModels_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                    table.ForeignKey(
                        name: "FK_roomScheduleModels_TeachingScheduleModel_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "TeachingScheduleModel",
                        principalColumn: "ScheduleID");
                });

            migrationBuilder.CreateTable(
                name: "studentScheduleInfoModels",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Bell1EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell2EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell3EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell4EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell5EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell6EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    Bell7EnrollmentCodeMod = table.Column<int>(type: "int", nullable: false),
                    AvesBellRoomCodeMod = table.Column<int>(type: "int", nullable: false),
                    LunchCodeMod = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentScheduleInfoModels", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_studentScheduleInfoModels_bellAttendanceModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "bellAttendanceModels",
                        principalColumn: "StudentId");
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
                    EnrollmentStudentId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentCourseId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "fastPassModels",
                columns: table => new
                {
                    FastPassIDMod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    StudentNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellNumber = table.Column<int>(type: "int", nullable: false),
                    StartLocationID = table.Column<int>(type: "int", nullable: false),
                    EndLocationID = table.Column<int>(type: "int", nullable: false),
                    CourseIDFromStudentSchedule = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fastPassModels", x => x.FastPassIDMod);
                    table.ForeignKey(
                        name: "FK_fastPassModels_roomLocationInfoModels_EndLocationID",
                        column: x => x.EndLocationID,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_fastPassModels_studentInfoModels_StudentID",
                        column: x => x.StudentID,
                        principalTable: "studentInfoModels",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK_fastPassModels_studentScheduleInfoModels_CourseIDFromStudentSchedule",
                        column: x => x.CourseIDFromStudentSchedule,
                        principalTable: "studentScheduleInfoModels",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_CourseRoomID",
                table: "activeCourseInfoModels",
                column: "CourseRoomID");

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_CourseTeacherID",
                table: "activeCourseInfoModels",
                column: "CourseTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_SubstituteSubID",
                table: "activeCourseInfoModels",
                column: "SubstituteSubID");

            migrationBuilder.CreateIndex(
                name: "IX_courseEnrollmentModels_EnrollmentCourseId",
                table: "courseEnrollmentModels",
                column: "EnrollmentCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_courseEnrollmentModels_EnrollmentStudentId",
                table: "courseEnrollmentModels",
                column: "EnrollmentStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_dailyAttendanceModels_StudentId",
                table: "dailyAttendanceModels",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_fastPassModels_CourseIDFromStudentSchedule",
                table: "fastPassModels",
                column: "CourseIDFromStudentSchedule");

            migrationBuilder.CreateIndex(
                name: "IX_fastPassModels_EndLocationID",
                table: "fastPassModels",
                column: "EndLocationID");

            migrationBuilder.CreateIndex(
                name: "IX_fastPassModels_StudentID",
                table: "fastPassModels",
                column: "StudentID");

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
                name: "IX_hallPassInfoModels_StudentID",
                table: "hallPassInfoModels",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_hallPassInfoModels_TeacherInfoModelTeacherID1",
                table: "hallPassInfoModels",
                column: "TeacherInfoModelTeacherID1");

            migrationBuilder.CreateIndex(
                name: "IX_passRequestInfoModels_HallPassAddressedBy",
                table: "passRequestInfoModels",
                column: "HallPassAddressedBy");

            migrationBuilder.CreateIndex(
                name: "IX_passRequestInfoModels_HallPassAssignedBy",
                table: "passRequestInfoModels",
                column: "HallPassAssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_passRequestInfoModels_StudentID",
                table: "passRequestInfoModels",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_roomLocationInfoModels_RoomAssignedToTeacherID",
                table: "roomLocationInfoModels",
                column: "RoomAssignedToTeacherID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roomScheduleModels_RoomId",
                table: "roomScheduleModels",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roomScheduleModels_ScheduleID",
                table: "roomScheduleModels",
                column: "ScheduleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roomScheduleModels_TeacherID",
                table: "roomScheduleModels",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_studentInfoModels_ActivationCodeId",
                table: "studentInfoModels",
                column: "ActivationCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_studentInfoModels_EASuportEaID",
                table: "studentInfoModels",
                column: "EASuportEaID");

            migrationBuilder.CreateIndex(
                name: "IX_studentInfoModels_StudentCounselorID",
                table: "studentInfoModels",
                column: "StudentCounselorID");

            migrationBuilder.CreateIndex(
                name: "IX_studentInfoModels_StudentEAID",
                table: "studentInfoModels",
                column: "StudentEAID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_substituteInfoModels_ManagedTeacherIdMod",
                table: "substituteInfoModels",
                column: "ManagedTeacherIdMod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_synnLabQRNodeModels_SynnlabRoomIDMod",
                table: "synnLabQRNodeModels",
                column: "SynnlabRoomIDMod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeachingScheduleModel_TeacherID",
                table: "TeachingScheduleModel",
                column: "TeacherID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courseEnrollmentModels");

            migrationBuilder.DropTable(
                name: "dailyAttendanceModels");

            migrationBuilder.DropTable(
                name: "dailyBellScheduleModels");

            migrationBuilder.DropTable(
                name: "developerInfoModels");

            migrationBuilder.DropTable(
                name: "extendedAvesModels");

            migrationBuilder.DropTable(
                name: "fastPassModels");

            migrationBuilder.DropTable(
                name: "hallPassInfoModels");

            migrationBuilder.DropTable(
                name: "passRequestInfoModels");

            migrationBuilder.DropTable(
                name: "pepRallyBellScheduleModels");

            migrationBuilder.DropTable(
                name: "roomScheduleModels");

            migrationBuilder.DropTable(
                name: "synnLabQRNodeModels");

            migrationBuilder.DropTable(
                name: "twoHrDelayBellScheduleModels");

            migrationBuilder.DropTable(
                name: "activeCourseInfoModels");

            migrationBuilder.DropTable(
                name: "studentScheduleInfoModels");

            migrationBuilder.DropTable(
                name: "adminInfoModels");

            migrationBuilder.DropTable(
                name: "attendanceOfficeMemberModels");

            migrationBuilder.DropTable(
                name: "lawEnforcementInfoModels");

            migrationBuilder.DropTable(
                name: "nurseInfoModels");

            migrationBuilder.DropTable(
                name: "TeachingScheduleModel");

            migrationBuilder.DropTable(
                name: "roomLocationInfoModels");

            migrationBuilder.DropTable(
                name: "substituteInfoModels");

            migrationBuilder.DropTable(
                name: "bellAttendanceModels");

            migrationBuilder.DropTable(
                name: "teacherInfoModels");

            migrationBuilder.DropTable(
                name: "studentInfoModels");

            migrationBuilder.DropTable(
                name: "activationModels");

            migrationBuilder.DropTable(
                name: "counselorModels");

            migrationBuilder.DropTable(
                name: "eASuportInfoModels");
        }
    }
}
