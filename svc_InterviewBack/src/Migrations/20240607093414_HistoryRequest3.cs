using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class HistoryRequest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots",
                column: "PreviousRequestStatusSnapshotId",
                principalTable: "RequestStatusSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots",
                column: "PreviousRequestStatusSnapshotId",
                principalTable: "RequestStatusSnapshots",
                principalColumn: "Id");
        }
    }
}
