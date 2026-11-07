using Microsoft.EntityFrameworkCore.Migrations;

namespace SAMS.Data.Migrations
{
    public partial class AddNullableStudentPin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentPin",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("UPDATE AspNetUsers SET StudentPin = NULL WHERE StudentPin = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AspNetUsers SET StudentPin = 0 WHERE StudentPin IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "StudentPin",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
