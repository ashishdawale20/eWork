using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderDerivedWordsToCrimeRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CrimeRegisters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            

            migrationBuilder.AddColumn<string>(
                name: "PersonToWord",
                table: "CrimeRegisters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonWord",
                table: "CrimeRegisters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProperWord",
                table: "CrimeRegisters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            
            migrationBuilder.DropColumn(
                name: "PersonToWord",
                table: "CrimeRegisters");

            migrationBuilder.DropColumn(
                name: "PersonWord",
                table: "CrimeRegisters");

            migrationBuilder.DropColumn(
                name: "ProperWord",
                table: "CrimeRegisters");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CrimeRegisters",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
