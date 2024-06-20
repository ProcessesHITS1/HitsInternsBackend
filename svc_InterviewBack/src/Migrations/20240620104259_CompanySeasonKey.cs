using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class CompanySeasonKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Companies_CompanyId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CompanyId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CompanyId",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanySeasonId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResultStatus",
                table: "RequestResult",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanySeasonId",
                table: "Positions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                columns: new[] { "Id", "SeasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CompanyId_CompanySeasonId",
                table: "Students",
                columns: new[] { "CompanyId", "CompanySeasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CompanyId_CompanySeasonId",
                table: "Positions",
                columns: new[] { "CompanyId", "CompanySeasonId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions",
                columns: new[] { "CompanyId", "CompanySeasonId" },
                principalTable: "Companies",
                principalColumns: new[] { "Id", "SeasonId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Companies_CompanyId_CompanySeasonId",
                table: "Students",
                columns: new[] { "CompanyId", "CompanySeasonId" },
                principalTable: "Companies",
                principalColumns: new[] { "Id", "SeasonId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Companies_CompanyId_CompanySeasonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CompanyId_CompanySeasonId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CompanyId_CompanySeasonId",
                table: "Positions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanySeasonId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CompanySeasonId",
                table: "Positions");

            migrationBuilder.AlterColumn<int>(
                name: "ResultStatus",
                table: "RequestResult",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CompanyId",
                table: "Students",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CompanyId",
                table: "Positions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId",
                table: "Positions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Companies_CompanyId",
                table: "Students",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
