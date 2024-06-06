using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class RequestHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "InterviewRequests",
                newName: "ResultStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestStatusSnapshotId",
                table: "InterviewRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestStatusSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestStatus = table.Column<int>(type: "integer", nullable: false),
                    InterviewRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreviousRequestStatusSnapshotId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                        column: x => x.InterviewRequestId,
                        principalTable: "InterviewRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                        column: x => x.PreviousRequestStatusSnapshotId,
                        principalTable: "RequestStatusSnapshots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_PreviousRequestStatusSnapshotId",
                table: "RequestStatusSnapshots",
                column: "PreviousRequestStatusSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestStatusSnapshots");

            migrationBuilder.DropColumn(
                name: "RequestStatusSnapshotId",
                table: "InterviewRequests");

            migrationBuilder.RenameColumn(
                name: "ResultStatus",
                table: "InterviewRequests",
                newName: "Status");
        }
    }
}
