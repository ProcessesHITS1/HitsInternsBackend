using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class StudentandSchoolStatusResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultStatus",
                table: "RequestResult",
                newName: "StudentResultStatus");

            migrationBuilder.AddColumn<int>(
                name: "SchoolResultStatus",
                table: "RequestResult",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolResultStatus",
                table: "RequestResult");

            migrationBuilder.RenameColumn(
                name: "StudentResultStatus",
                table: "RequestResult",
                newName: "ResultStatus");
        }
    }
}
