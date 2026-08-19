using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL_SmartAppraisal.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseStudyReviewAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "CaseStudies",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ReviewComment",
                table: "CaseStudies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewedBy",
                table: "CaseStudies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedDate",
                table: "CaseStudies",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewComment",
                table: "CaseStudies");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "CaseStudies");

            migrationBuilder.DropColumn(
                name: "ReviewedDate",
                table: "CaseStudies");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "CaseStudies",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
