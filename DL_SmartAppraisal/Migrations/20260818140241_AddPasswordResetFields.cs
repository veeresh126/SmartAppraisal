using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL_SmartAppraisal.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                table: "UserDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseStudies_CreatedBy",
                table: "CaseStudies",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseStudies_UserDetails_CreatedBy",
                table: "CaseStudies",
                column: "CreatedBy",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseStudies_UserDetails_CreatedBy",
                table: "CaseStudies");

            migrationBuilder.DropIndex(
                name: "IX_CaseStudies_CreatedBy",
                table: "CaseStudies");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                table: "UserDetails");
        }
    }
}
