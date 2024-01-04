using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenAgentsAndHistoryRecordsAndAddExcludedTablesInConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleanEventTables",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "BackUpHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ExcludedTablesJsonList",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BackUpHistory_AgentId",
                table: "BackUpHistory",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackUpHistory_Agents_AgentId",
                table: "BackUpHistory",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentKey",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackUpHistory_Agents_AgentId",
                table: "BackUpHistory");

            migrationBuilder.DropIndex(
                name: "IX_BackUpHistory_AgentId",
                table: "BackUpHistory");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "BackUpHistory");

            migrationBuilder.DropColumn(
                name: "ExcludedTablesJsonList",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<bool>(
                name: "CleanEventTables",
                table: "BackUpConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
