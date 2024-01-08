using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion11202403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "studentGradYearMod",
                table: "studentInfoModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "studentGradYearMod",
                table: "studentInfoModels");
        }
    }
}
