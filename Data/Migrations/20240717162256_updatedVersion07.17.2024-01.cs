using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0717202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentIdMod",
                table: "StudentLocationModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentLocationModels_StudentIdMod",
                table: "StudentLocationModels",
                column: "StudentIdMod",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLocationModels_StudentInfoModels_StudentIdMod",
                table: "StudentLocationModels",
                column: "StudentIdMod",
                principalTable: "StudentInfoModels",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentLocationModels_StudentInfoModels_StudentIdMod",
                table: "StudentLocationModels");

            migrationBuilder.DropIndex(
                name: "IX_StudentLocationModels_StudentIdMod",
                table: "StudentLocationModels");

            migrationBuilder.DropColumn(
                name: "StudentIdMod",
                table: "StudentLocationModels");
        }
    }
}
