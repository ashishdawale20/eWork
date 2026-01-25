using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Jawans");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "Jawans",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offices");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Jawans",
                newName: "OfficeId");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Jawans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
