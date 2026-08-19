using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL_SmartAppraisal.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseStudyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseStudies",
                columns: table => new
                {
                    CaseStudyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseStudyText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStudies", x => x.CaseStudyId);
                });

            migrationBuilder.CreateTable(
                name: "CaseStudySolutions",
                columns: table => new
                {
                    CaseStudySolutionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseStudyId = table.Column<int>(type: "int", nullable: false),
                    SolutionNumber = table.Column<int>(type: "int", nullable: false),
                    SolutionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStudySolutions", x => x.CaseStudySolutionId);
                    table.ForeignKey(
                        name: "FK_CaseStudySolutions_CaseStudies_CaseStudyId",
                        column: x => x.CaseStudyId,
                        principalTable: "CaseStudies",
                        principalColumn: "CaseStudyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseStudyCompetencies",
                columns: table => new
                {
                    CaseStudyCompetencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseStudySolutionId = table.Column<int>(type: "int", nullable: false),
                    CompetencyName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseStudyCompetencies", x => x.CaseStudyCompetencyId);
                    table.ForeignKey(
                        name: "FK_CaseStudyCompetencies_CaseStudySolutions_CaseStudySolutionId",
                        column: x => x.CaseStudySolutionId,
                        principalTable: "CaseStudySolutions",
                        principalColumn: "CaseStudySolutionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseStudyCompetencies_CaseStudySolutionId",
                table: "CaseStudyCompetencies",
                column: "CaseStudySolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseStudySolutions_CaseStudyId",
                table: "CaseStudySolutions",
                column: "CaseStudyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseStudyCompetencies");

            migrationBuilder.DropTable(
                name: "CaseStudySolutions");

            migrationBuilder.DropTable(
                name: "CaseStudies");
        }
    }
}
