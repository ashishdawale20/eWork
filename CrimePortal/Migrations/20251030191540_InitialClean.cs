using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Panchas_CrimeRegister_CrimeRegisterId",
                table: "Panchas");

            migrationBuilder.DropForeignKey(
                name: "FK_Panchas_CrimeRegister_CrimeRegisterId1",
                table: "Panchas");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_CrimeRegister_CrimeRegisterId",
                table: "Samples");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_CrimeRegister_CrimeRegisterId1",
                table: "Samples");

            migrationBuilder.DropForeignKey(
                name: "FK_SeizedItems_CrimeRegister_CrimeRegisterId",
                table: "SeizedItems");

            migrationBuilder.DropIndex(
                name: "IX_Samples_CrimeRegisterId1",
                table: "Samples");

            migrationBuilder.DropIndex(
                name: "IX_Panchas_CrimeRegisterId1",
                table: "Panchas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrimeRegister",
                table: "CrimeRegister");

            migrationBuilder.DropColumn(
                name: "CrimeRegisterId1",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "CrimeRegisterId1",
                table: "Panchas");

            migrationBuilder.RenameTable(
                name: "CrimeRegister",
                newName: "CrimeRegisters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrimeRegisters",
                table: "CrimeRegisters",
                column: "CrimeRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Panchas_CrimeRegisters_CrimeRegisterId",
                table: "Panchas",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegisters",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_CrimeRegisters_CrimeRegisterId",
                table: "Samples",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegisters",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeizedItems_CrimeRegisters_CrimeRegisterId",
                table: "SeizedItems",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegisters",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Panchas_CrimeRegisters_CrimeRegisterId",
                table: "Panchas");

            migrationBuilder.DropForeignKey(
                name: "FK_Samples_CrimeRegisters_CrimeRegisterId",
                table: "Samples");

            migrationBuilder.DropForeignKey(
                name: "FK_SeizedItems_CrimeRegisters_CrimeRegisterId",
                table: "SeizedItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CrimeRegisters",
                table: "CrimeRegisters");

            migrationBuilder.RenameTable(
                name: "CrimeRegisters",
                newName: "CrimeRegister");

            migrationBuilder.AddColumn<int>(
                name: "CrimeRegisterId1",
                table: "Samples",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CrimeRegisterId1",
                table: "Panchas",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CrimeRegister",
                table: "CrimeRegister",
                column: "CrimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_CrimeRegisterId1",
                table: "Samples",
                column: "CrimeRegisterId1");

            migrationBuilder.CreateIndex(
                name: "IX_Panchas_CrimeRegisterId1",
                table: "Panchas",
                column: "CrimeRegisterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Panchas_CrimeRegister_CrimeRegisterId",
                table: "Panchas",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegister",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Panchas_CrimeRegister_CrimeRegisterId1",
                table: "Panchas",
                column: "CrimeRegisterId1",
                principalTable: "CrimeRegister",
                principalColumn: "CrimeRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_CrimeRegister_CrimeRegisterId",
                table: "Samples",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegister",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Samples_CrimeRegister_CrimeRegisterId1",
                table: "Samples",
                column: "CrimeRegisterId1",
                principalTable: "CrimeRegister",
                principalColumn: "CrimeRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeizedItems_CrimeRegister_CrimeRegisterId",
                table: "SeizedItems",
                column: "CrimeRegisterId",
                principalTable: "CrimeRegister",
                principalColumn: "CrimeRegisterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
