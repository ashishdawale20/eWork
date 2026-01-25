using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrimePortal.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrimeRegister",
                columns: table => new
                {
                    CrimeRegisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrimeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeFrom = table.Column<TimeSpan>(type: "time", nullable: true),
                    TimeTo = table.Column<TimeSpan>(type: "time", nullable: true),
                    Complainant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccusedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccusedAge = table.Column<int>(type: "int", nullable: true),
                    AccusedGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccusedRelativeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccusedAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfIncident = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Act = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Court = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigatingOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigatingOfficerRank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficerRank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleNumber = table.Column<int>(type: "int", nullable: true),
                    TotalSeizedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeRegister", x => x.CrimeRegisterId);
                });

            migrationBuilder.CreateTable(
                name: "Panchas",
                columns: table => new
                {
                    PanchaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "int", nullable: false),
                    CrimeRegisterId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panchas", x => x.PanchaId);
                    table.ForeignKey(
                        name: "FK_Panchas_CrimeRegister_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegister",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Panchas_CrimeRegister_CrimeRegisterId1",
                        column: x => x.CrimeRegisterId1,
                        principalTable: "CrimeRegister",
                        principalColumn: "CrimeRegisterId");
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "int", nullable: false),
                    CrimeRegisterId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleId);
                    table.ForeignKey(
                        name: "FK_Samples_CrimeRegister_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegister",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Samples_CrimeRegister_CrimeRegisterId1",
                        column: x => x.CrimeRegisterId1,
                        principalTable: "CrimeRegister",
                        principalColumn: "CrimeRegisterId");
                });

            migrationBuilder.CreateTable(
                name: "SeizedItems",
                columns: table => new
                {
                    SeizedItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApproxValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrimeRegisterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeizedItems", x => x.SeizedItemId);
                    table.ForeignKey(
                        name: "FK_SeizedItems_CrimeRegister_CrimeRegisterId",
                        column: x => x.CrimeRegisterId,
                        principalTable: "CrimeRegister",
                        principalColumn: "CrimeRegisterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Panchas_CrimeRegisterId",
                table: "Panchas",
                column: "CrimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Panchas_CrimeRegisterId1",
                table: "Panchas",
                column: "CrimeRegisterId1");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_CrimeRegisterId",
                table: "Samples",
                column: "CrimeRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_CrimeRegisterId1",
                table: "Samples",
                column: "CrimeRegisterId1");

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
                name: "CrimeRegister");
        }
    }
}
