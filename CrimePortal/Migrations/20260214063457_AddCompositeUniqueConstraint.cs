using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CrimeRegisters_Year_CaseNumber_UserId",
                table: "CrimeRegisters",
                columns: new[] { "Year", "CaseNumber", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrimeRegisters_Year_CaseNumber_UserId",
                table: "CrimeRegisters");
        }
    }
}
