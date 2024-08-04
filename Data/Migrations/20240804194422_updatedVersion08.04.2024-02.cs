using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0804202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentLocationModels",
                table: "StudentLocationModels",
                column: "StudentIdMod");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentLocationModels",
                table: "StudentLocationModels");
        }
    }
}
