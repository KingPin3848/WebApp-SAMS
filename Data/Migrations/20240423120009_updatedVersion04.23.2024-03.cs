using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0423202403 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BellAttendanceId",
                table: "bellAttendanceModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels",
                column: "BellAttendanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bellAttendanceModels",
                table: "bellAttendanceModels");

            migrationBuilder.DropColumn(
                name: "BellAttendanceId",
                table: "bellAttendanceModels");
        }
    }
}
