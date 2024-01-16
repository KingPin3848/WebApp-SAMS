using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion116202401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EaStudentManaged",
                table: "eASuportInfoModels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AdminEmailMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminFirstNameMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLabelMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLastNameMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminMiddleNameMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminPhoneMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminPreferredNameMod",
                table: "adminInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminEmailMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminFirstNameMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminLabelMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminLastNameMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminMiddleNameMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminPhoneMod",
                table: "adminInfoModels");

            migrationBuilder.DropColumn(
                name: "AdminPreferredNameMod",
                table: "adminInfoModels");

            migrationBuilder.AlterColumn<int>(
                name: "EaStudentManaged",
                table: "eASuportInfoModels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
