using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class HistoryRequest4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_RequestStatusSnapshots_PreviousRequestStatusSnapshotId",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropColumn(
                name: "PreviousRequestStatusSnapshotId",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropColumn(
                name: "RequestStatusSnapshotId",
                table: "InterviewRequests");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId",
                principalTable: "InterviewRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots");

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousRequestStatusSnapshotId",
                table: "RequestStatusSnapshots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestStatusSnapshotId",
                table: "InterviewRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_PreviousRequestStatusSnapshotId",
                table: "RequestStatusSnapshots",
                column: "PreviousRequestStatusSnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId",
                principalTable: "InterviewRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusSnapshots_PreviousReque~",
                table: "RequestStatusSnapshots",
                column: "PreviousRequestStatusSnapshotId",
                principalTable: "RequestStatusSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
