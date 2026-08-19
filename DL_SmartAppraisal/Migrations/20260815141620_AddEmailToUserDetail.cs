using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DL_SmartAppraisal.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToUserDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserDetails");
        }
    }
}
