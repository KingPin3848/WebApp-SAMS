using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0218202403 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriBell2CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell3CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell4CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell5CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell6CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell7CourseIDMod",
                table: "sem2StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell2CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell3CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell4CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell5CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell6CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FriBell7CourseIDMod",
                table: "sem1StudSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriBell2CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell3CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell4CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell5CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell6CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell7CourseIDMod",
                table: "sem2StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell2CourseIDMod",
                table: "sem1StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell3CourseIDMod",
                table: "sem1StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell4CourseIDMod",
                table: "sem1StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell5CourseIDMod",
                table: "sem1StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell6CourseIDMod",
                table: "sem1StudSchedules");

            migrationBuilder.DropColumn(
                name: "FriBell7CourseIDMod",
                table: "sem1StudSchedules");
        }
    }
}
