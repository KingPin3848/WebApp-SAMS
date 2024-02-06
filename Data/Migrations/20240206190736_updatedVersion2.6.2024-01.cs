using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion26202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activeCourseInfoModels_substituteInfoModels_SubstituteSubID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropForeignKey(
                name: "FK_teacherInfoModels_substituteInfoModels_AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropTable(
                name: "substituteInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_teacherInfoModels_AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropIndex(
                name: "IX_activeCourseInfoModels_SubstituteSubID",
                table: "activeCourseInfoModels");

            migrationBuilder.DropColumn(
                name: "AssignedSubID",
                table: "teacherInfoModels");

            migrationBuilder.DropColumn(
                name: "SubstituteSubID",
                table: "activeCourseInfoModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedSubID",
                table: "teacherInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubstituteSubID",
                table: "activeCourseInfoModels",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "substituteInfoModels",
                columns: table => new
                {
                    SubID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ManagedTeacherIdMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubEmailMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubFirstNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubLastNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubMiddleNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubPhoneMod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubPreferredNameMod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_substituteInfoModels", x => x.SubID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teacherInfoModels_AssignedSubID",
                table: "teacherInfoModels",
                column: "AssignedSubID");

            migrationBuilder.CreateIndex(
                name: "IX_activeCourseInfoModels_SubstituteSubID",
                table: "activeCourseInfoModels",
                column: "SubstituteSubID");

            migrationBuilder.AddForeignKey(
                name: "FK_activeCourseInfoModels_substituteInfoModels_SubstituteSubID",
                table: "activeCourseInfoModels",
                column: "SubstituteSubID",
                principalTable: "substituteInfoModels",
                principalColumn: "SubID");

            migrationBuilder.AddForeignKey(
                name: "FK_teacherInfoModels_substituteInfoModels_AssignedSubID",
                table: "teacherInfoModels",
                column: "AssignedSubID",
                principalTable: "substituteInfoModels",
                principalColumn: "SubID");
        }
    }
}
