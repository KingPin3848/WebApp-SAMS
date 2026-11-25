using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAMS.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261107203500_RequireStudentPin")]
    public partial class RequireStudentPin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE AspNetUsers SET StudentPin = -1 WHERE StudentPin IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "StudentPin",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudentPin",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("UPDATE AspNetUsers SET StudentPin = NULL WHERE StudentPin = -1");
        }
    }
}
