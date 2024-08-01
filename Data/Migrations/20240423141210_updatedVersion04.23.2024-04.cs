using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0423202404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentIdMod",
                table: "bellAttendanceModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentIdMod",
                table: "bellAttendanceModels");
        }
    }
}
