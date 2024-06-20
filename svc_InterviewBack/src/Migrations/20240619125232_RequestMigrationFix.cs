using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class RequestMigrationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "InterviewRequests");

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
                    table.ForeignKey(
                        name: "FK_RequestResult_InterviewRequests_Id",
                        column: x => x.Id,
                        principalTable: "InterviewRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatusTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatusSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestStatusTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewRequestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestStatusSnapshots_InterviewRequests_InterviewRequestId",
                        column: x => x.InterviewRequestId,
                        principalTable: "InterviewRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestStatusSnapshots_RequestStatusTemplates_RequestStatus~",
                        column: x => x.RequestStatusTemplateId,
                        principalTable: "RequestStatusTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatusTemplateSeason",
                columns: table => new
                {
                    RequestStatusTemplatesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeasonsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatusTemplateSeason", x => new { x.RequestStatusTemplatesId, x.SeasonsId });
                    table.ForeignKey(
                        name: "FK_RequestStatusTemplateSeason_RequestStatusTemplates_RequestS~",
                        column: x => x.RequestStatusTemplatesId,
                        principalTable: "RequestStatusTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestStatusTemplateSeason_Seasons_SeasonsId",
                        column: x => x.SeasonsId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_InterviewRequestId",
                table: "RequestStatusSnapshots",
                column: "InterviewRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusSnapshots_RequestStatusTemplateId",
                table: "RequestStatusSnapshots",
                column: "RequestStatusTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusTemplates_Name",
                table: "RequestStatusTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestStatusTemplateSeason_SeasonsId",
                table: "RequestStatusTemplateSeason",
                column: "SeasonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestResult");

            migrationBuilder.DropTable(
                name: "RequestStatusSnapshots");

            migrationBuilder.DropTable(
                name: "RequestStatusTemplateSeason");

            migrationBuilder.DropTable(
                name: "RequestStatusTemplates");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "InterviewRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
