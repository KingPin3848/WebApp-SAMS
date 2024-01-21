using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    public partial class restructurization117202401 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCode",
                table: "studentInfoModels");

            migrationBuilder.DropTable(
                name: "activationModels");

            migrationBuilder.DropIndex(
                name: "IX_studentInfoModels_ActivationCode",
                table: "studentInfoModels");

            migrationBuilder.AddColumn<string>(
                name: "ActivationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserExperienceEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserExperienceEnabled",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "activationModels",
                columns: table => new
                {
                    StudId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activationModels", x => x.StudId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_studentInfoModels_ActivationCode",
                table: "studentInfoModels",
                column: "ActivationCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_studentInfoModels_activationModels_ActivationCode",
                table: "studentInfoModels",
                column: "ActivationCode",
                principalTable: "activationModels",
                principalColumn: "StudId");
        }
    }
}
