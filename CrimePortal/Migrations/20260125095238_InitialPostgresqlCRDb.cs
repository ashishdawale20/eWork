using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresqlCRDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrimeRegisters",
                columns: table => new
                {
                    CrimeRegisterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    CaseNumber = table.Column<string>(type: "text", nullable: false),
                    CrimeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeFrom = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TimeTo = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Complainant = table.Column<string>(type: "text", nullable: true),
                    AccusedName = table.Column<string>(type: "text", nullable: true),
                    AccusedAge = table.Column<int>(type: "integer", nullable: true),
                    AccusedGender = table.Column<string>(type: "text", nullable: true),
                    AccusedRelativeName = table.Column<string>(type: "text", nullable: true),
                    AccusedAddress = table.Column<string>(type: "text", nullable: true),
                    PlaceOfIncident = table.Column<string>(type: "text", nullable: true),
                    Act = table.Column<string>(type: "text", nullable: true),
                    Court = table.Column<string>(type: "text", nullable: true),
                    PoliceStation = table.Column<string>(type: "text", nullable: true),
                    InvestigatingOfficerName = table.Column<string>(type: "text", nullable: true),
                    InvestigatingOfficerRank = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    OfficerRank = table.Column<string>(type: "text", nullable: true),
                    OfficeName = table.Column<string>(type: "text", nullable: true),
                    OfficeAddress = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    Division = table.Column<string>(type: "text", nullable: true),
                    CarrierName = table.Column<string>(type: "text", nullable: true),
                    SampleNumber = table.Column<int>(type: "integer", nullable: true),
                    TotalSeizedValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    HandBhattiLiters = table.Column<decimal>(type: "numeric", nullable: true),
                    MohasavaLiters = table.Column<decimal>(type: "numeric", nullable: true),
                    DeshiLiquorLiters = table.Column<decimal>(type: "numeric", nullable: true),
                    ForeignLiquorLiters = table.Column<decimal>(type: "numeric", nullable: true),
                    BeerLiters = table.Column<decimal>(type: "numeric", nullable: true),
                    PersonWord = table.Column<string>(type: "text", nullable: true),
                    PersonToWord = table.Column<string>(type: "text", nullable: true),
                    ProperWord = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeRegisters", x => x.CrimeRegisterId);
                });

            migrationBuilder.CreateTable(
                name: "Panchas",
                columns: table => new
                {
                    PanchaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panchas", x => x.PanchaId);
                    table.ForeignKey(
                        name: "FK_Panchas_CrimeRegisters_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegisters",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SampleType = table.Column<string>(type: "text", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleId);
                    table.ForeignKey(
                        name: "FK_Samples_CrimeRegisters_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegisters",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeizedItems",
                columns: table => new
                {
                    SeizedItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyClass = table.Column<string>(type: "text", nullable: true),
                    PropertyType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ApproxValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeizedItems", x => x.SeizedItemId);
                    table.ForeignKey(
                        name: "FK_SeizedItems_CrimeRegisters_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegisters",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrimeRegisters_Year_CaseNumber",
                table: "CrimeRegisters",
                columns: new[] { "Year", "CaseNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Panchas_CrimeRegisterId",
                table: "Panchas",
                column: "CrimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_CrimeRegisterId",
                table: "Samples",
                column: "CrimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_SeizedItems_CrimeRegisterId",
                table: "SeizedItems",
                column: "CrimeRegisterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Panchas");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SeizedItems");

            migrationBuilder.DropTable(
                name: "CrimeRegisters");
        }
    }
}
