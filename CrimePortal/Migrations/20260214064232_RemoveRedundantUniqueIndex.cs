using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrimeRegisters_Year_CaseNumber",
                table: "CrimeRegisters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CrimeRegisters_Year_CaseNumber",
                table: "CrimeRegisters",
                columns: new[] { "Year", "CaseNumber" },
                unique: true);
        }
    }
}
