using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class RequestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultStatus",
                table: "InterviewRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestResultId",
                table: "InterviewRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OfferGiven = table.Column<bool>(type: "boolean", nullable: false),
                    ResultStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResult", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRequests_RequestResultId",
                table: "InterviewRequests",
                column: "RequestResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewRequests_RequestResult_RequestResultId",
                table: "InterviewRequests",
                column: "RequestResultId",
                principalTable: "RequestResult",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewRequests_RequestResult_RequestResultId",
                table: "InterviewRequests");

            migrationBuilder.DropTable(
                name: "RequestResult");

            migrationBuilder.DropIndex(
                name: "IX_InterviewRequests_RequestResultId",
                table: "InterviewRequests");

            migrationBuilder.DropColumn(
                name: "RequestResultId",
                table: "InterviewRequests");

            migrationBuilder.AddColumn<int>(
                name: "ResultStatus",
                table: "InterviewRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
