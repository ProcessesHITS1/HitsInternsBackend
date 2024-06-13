using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class RequestStatusTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "RequestStatusSnapshots");

            migrationBuilder.AddColumn<string>(
                name: "RequestStatusTemplateName",
                table: "RequestStatusSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RequestStatusTemplates",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    SeasonId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusTemplates", x => x.Name);
                    table.ForeignKey(
                        name: "FK_RequestStatusTemplates_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_RequestStatusTemplateName",
                table: "RequestStatusSnapshots",
                column: "RequestStatusTemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusTemplates_SeasonId",
                table: "RequestStatusTemplates",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusTemplates_RequestStatus~",
                table: "RequestStatusSnapshots",
                column: "RequestStatusTemplateName",
                principalTable: "RequestStatusTemplates",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestStatusSnapshots_RequestStatusTemplates_RequestStatus~",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropTable(
                name: "RequestStatusTemplates");

            migrationBuilder.DropIndex(
                name: "IX_RequestStatusSnapshots_RequestStatusTemplateName",
                table: "RequestStatusSnapshots");

            migrationBuilder.DropColumn(
                name: "RequestStatusTemplateName",
                table: "RequestStatusSnapshots");

            migrationBuilder.AddColumn<int>(
                name: "RequestStatus",
                table: "RequestStatusSnapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
