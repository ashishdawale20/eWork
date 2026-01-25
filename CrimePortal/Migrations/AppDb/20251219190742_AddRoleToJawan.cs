using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddRoleToJawan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Jawans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Jawans");
        }
    }
}
