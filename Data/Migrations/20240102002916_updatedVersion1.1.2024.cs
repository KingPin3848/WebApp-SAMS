using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class updatedVersion112024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationModel");

            migrationBuilder.AddColumn<string>(
                name: "studentMiddleNameMod",
                table: "studentScheduleInfoModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "studentMiddleNameMod",
                table: "studentScheduleInfoModels");

            migrationBuilder.CreateTable(
                name: "ActivationModel",
                columns: table => new
                {
                    ActivationCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EnableUserExperience = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationModel", x => x.ActivationCode);
                });
        }
    }
}
