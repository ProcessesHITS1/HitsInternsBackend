using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class StudentsPk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewRequests_Students_StudentId",
                table: "InterviewRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_InterviewRequests_StudentId",
                table: "InterviewRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentSeasonId",
                table: "InterviewRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                columns: new[] { "Id", "SeasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CompanyId",
                table: "Students",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRequests_StudentId_StudentSeasonId",
                table: "InterviewRequests",
                columns: new[] { "StudentId", "StudentSeasonId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewRequests_Students_StudentId_StudentSeasonId",
                table: "InterviewRequests",
                columns: new[] { "StudentId", "StudentSeasonId" },
                principalTable: "Students",
                principalColumns: new[] { "Id", "SeasonId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Companies_CompanyId",
                table: "Students",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewRequests_Students_StudentId_StudentSeasonId",
                table: "InterviewRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Companies_CompanyId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CompanyId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_InterviewRequests_StudentId_StudentSeasonId",
                table: "InterviewRequests");

            migrationBuilder.DropColumn(
                name: "StudentSeasonId",
                table: "InterviewRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewRequests_StudentId",
                table: "InterviewRequests",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewRequests_Students_StudentId",
                table: "InterviewRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
