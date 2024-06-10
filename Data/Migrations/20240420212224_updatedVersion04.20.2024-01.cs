using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedVersion0420202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChosenBellSchedModel",
                table: "ChosenBellSchedModel");

            migrationBuilder.RenameTable(
                name: "ChosenBellSchedModel",
                newName: "chosenBellSchedModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chosenBellSchedModels",
                table: "chosenBellSchedModels",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CustomSchedules",
                columns: table => new
                {
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    BellName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomSchedules", x => x.StartTime);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chosenBellSchedModels",
                table: "chosenBellSchedModels");

            migrationBuilder.RenameTable(
                name: "chosenBellSchedModels",
                newName: "ChosenBellSchedModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChosenBellSchedModel",
                table: "ChosenBellSchedModel",
                column: "Id");
        }
    }
}
