using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion111202401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCodeId",
                table: "studentInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activationModels",
                table: "activationModels");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "activationModels");

            migrationBuilder.AddColumn<string>(
                name: "CourseTaughtDays",
                table: "activeCourseInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "StudId",
                table: "activationModels",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activationModels",
                table: "activationModels",
                column: "StudId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCodeId",
                table: "studentInfoModels",
                column: "ActivationCodeId",
                principalTable: "activationModels",
                principalColumn: "StudId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCodeId",
                table: "studentInfoModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activationModels",
                table: "activationModels");

            migrationBuilder.DropColumn(
                name: "CourseTaughtDays",
                table: "activeCourseInfoModels");

            migrationBuilder.AlterColumn<int>(
                name: "StudId",
                table: "activationModels",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CodeId",
                table: "activationModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activationModels",
                table: "activationModels",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCodeId",
                table: "studentInfoModels",
                column: "ActivationCodeId",
                principalTable: "activationModels",
                principalColumn: "CodeId");
        }
    }
}
