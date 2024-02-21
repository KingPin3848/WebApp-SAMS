using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0218202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomScheduleModel");

            migrationBuilder.DropTable(
                name: "TeachingScheduleModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomScheduleModel",
                columns: table => new
                {
                    RoomScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ScheduleID = table.Column<int>(type: "int", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomScheduleModel", x => x.RoomScheduleID);
                    table.ForeignKey(
                        name: "FK_RoomScheduleModel_TeachingScheduleModel_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "TeachingScheduleModel",
                        principalColumn: "ScheduleID");
                    table.ForeignKey(
                        name: "FK_RoomScheduleModel_roomLocationInfoModels_RoomId",
                        column: x => x.RoomId,
                        principalTable: "roomLocationInfoModels",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_RoomScheduleModel_teacherInfoModels_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "teacherInfoModels",
                        principalColumn: "TeacherID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomScheduleModel_RoomId",
                table: "RoomScheduleModel",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomScheduleModel_ScheduleID",
                table: "RoomScheduleModel",
                column: "ScheduleID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomScheduleModel_TeacherID",
                table: "RoomScheduleModel",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingScheduleModel_TeacherID",
                table: "TeachingScheduleModel",
                column: "TeacherID",
                unique: true);
        }
    }
}
