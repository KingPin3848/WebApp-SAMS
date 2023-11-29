using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class newModelsAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activeCourseInfoModels",
                columns: table => new
                {
                    courseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseTeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseTeacherEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roomNumber = table.Column<int>(type: "int", nullable: false),
                    wingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    courseBellNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activeCourseInfoModels", x => x.courseId);
                });

            migrationBuilder.CreateTable(
                name: "adminInfoModels",
                columns: table => new
                {
                    adminID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminInfoModels", x => x.adminID);
                });

            migrationBuilder.CreateTable(
                name: "attendanceOfficeMemberModels",
                columns: table => new
                {
                    aoMemberID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    aoMemberFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aoMemberMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aoMemberLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aoMemberPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aoMemberEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    aoMemberPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendanceOfficeMemberModels", x => x.aoMemberID);
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
                    developerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    developerFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    developerMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    developerLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    developerPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    developerEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developerInfoModels", x => x.developerID);
                });

            migrationBuilder.CreateTable(
                name: "eASuportInfoModels",
                columns: table => new
                {
                    eaID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    eaFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eaStudentManaged = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eASuportInfoModels", x => x.eaID);
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
                name: "fastPassModels",
                columns: table => new
                {
                    fastPassIDMod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    studentID = table.Column<int>(type: "int", nullable: false),
                    studentNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    bellNumber = table.Column<int>(type: "int", nullable: false),
                    startLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fastPassModels", x => x.fastPassIDMod);
                });

            migrationBuilder.CreateTable(
                name: "hallPassInfoModels",
                columns: table => new
                {
                    HallPassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    studentID = table.Column<int>(type: "int", nullable: false),
                    studentNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    bellNumber = table.Column<int>(type: "int", nullable: false),
                    hallPassAssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hallPassAddressee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hallPassInfoModels", x => x.HallPassID);
                });

            migrationBuilder.CreateTable(
                name: "lawEnforcementInfoModels",
                columns: table => new
                {
                    lawenfID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    laweFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    laweMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    laweLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lawePreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    laweEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lawePhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lawEnforcementInfoModels", x => x.lawenfID);
                });

            migrationBuilder.CreateTable(
                name: "nurseInfoModels",
                columns: table => new
                {
                    nurseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nurseFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nurseMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nurseLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nursePreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nurseEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nursePhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nurseInfoModels", x => x.nurseID);
                });

            migrationBuilder.CreateTable(
                name: "passRequestInfoModels",
                columns: table => new
                {
                    passRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentID = table.Column<int>(type: "int", nullable: false),
                    studentNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hallPassAssignedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hallPassAddressee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    bellNumber = table.Column<int>(type: "int", nullable: false),
                    startLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    requestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passRequestInfoModels", x => x.passRequestId);
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
                name: "roomLocationInfoModels",
                columns: table => new
                {
                    roomNumberMod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    wingNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roomCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomLocationInfoModels", x => x.roomNumberMod);
                });

            migrationBuilder.CreateTable(
                name: "studentInfoModels",
                columns: table => new
                {
                    studentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentCounselorNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentCounselorEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentEANameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentEAEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentguard1NameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentguard1EmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentguard2NameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentguard2EmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentInfoModels", x => x.studentID);
                });

            migrationBuilder.CreateTable(
                name: "studentScheduleInfoModels",
                columns: table => new
                {
                    studentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell1CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell2CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell3CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell4CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell5CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell6CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bell7CourseCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    avesBellRoomCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lunchCodeMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentScheduleInfoModels", x => x.studentID);
                });

            migrationBuilder.CreateTable(
                name: "substituteInfoModels",
                columns: table => new
                {
                    subID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    subFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    managedTeacherMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_substituteInfoModels", x => x.subID);
                });

            migrationBuilder.CreateTable(
                name: "synnLabQRNodeModels",
                columns: table => new
                {
                    scannerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    synnlabRoomMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    synnlabRoomSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scannerMacAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modelNumberMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scannerDeviceIPAddressMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scannerLabelMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_synnLabQRNodeModels", x => x.scannerID);
                });

            migrationBuilder.CreateTable(
                name: "teacherInfoModels",
                columns: table => new
                {
                    teacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    teacherFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    teacherPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacherInfoModels", x => x.teacherID);
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activeCourseInfoModels");

            migrationBuilder.DropTable(
                name: "adminInfoModels");

            migrationBuilder.DropTable(
                name: "attendanceOfficeMemberModels");

            migrationBuilder.DropTable(
                name: "dailyBellScheduleModels");

            migrationBuilder.DropTable(
                name: "developerInfoModels");

            migrationBuilder.DropTable(
                name: "eASuportInfoModels");

            migrationBuilder.DropTable(
                name: "extendedAvesModels");

            migrationBuilder.DropTable(
                name: "fastPassModels");

            migrationBuilder.DropTable(
                name: "hallPassInfoModels");

            migrationBuilder.DropTable(
                name: "lawEnforcementInfoModels");

            migrationBuilder.DropTable(
                name: "nurseInfoModels");

            migrationBuilder.DropTable(
                name: "passRequestInfoModels");

            migrationBuilder.DropTable(
                name: "pepRallyBellScheduleModels");

            migrationBuilder.DropTable(
                name: "roomLocationInfoModels");

            migrationBuilder.DropTable(
                name: "studentInfoModels");

            migrationBuilder.DropTable(
                name: "studentScheduleInfoModels");

            migrationBuilder.DropTable(
                name: "substituteInfoModels");

            migrationBuilder.DropTable(
                name: "synnLabQRNodeModels");

            migrationBuilder.DropTable(
                name: "teacherInfoModels");

            migrationBuilder.DropTable(
                name: "twoHrDelayBellScheduleModels");
        }
    }
}
