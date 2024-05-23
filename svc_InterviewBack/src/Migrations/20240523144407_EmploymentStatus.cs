using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class EmploymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatus",
                table: "Students",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "Students");
        }
    }
}
