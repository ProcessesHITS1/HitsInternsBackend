using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace svc_InterviewBack.Migrations
{
    /// <inheritdoc />
    public partial class PositionAddCompanyReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanySeasonId",
                table: "Positions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Positions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions",
                columns: new[] { "CompanyId", "CompanySeasonId" },
                principalTable: "Companies",
                principalColumns: new[] { "Id", "SeasonId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanySeasonId",
                table: "Positions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Positions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Companies_CompanyId_CompanySeasonId",
                table: "Positions",
                columns: new[] { "CompanyId", "CompanySeasonId" },
                principalTable: "Companies",
                principalColumns: new[] { "Id", "SeasonId" });
        }
    }
}
