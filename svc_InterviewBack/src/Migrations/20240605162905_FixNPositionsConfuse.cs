using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class FixNPositionsConfuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NPositions",
                table: "Positions",
                newName: "NSeats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NSeats",
                table: "Positions",
                newName: "NPositions");
        }
    }
}
