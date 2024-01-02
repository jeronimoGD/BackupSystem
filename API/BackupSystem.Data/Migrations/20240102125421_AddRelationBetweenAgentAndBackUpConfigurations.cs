using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenAgentAndBackUpConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Agents",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "PeriodicBackUpType",
                table: "BackUpConfigurations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "BackUpSize",
                table: "BackUpHistory",
                newName: "BackUpSizeInMB");

            migrationBuilder.RenameColumn(
                name: "ConnectionKey",
                table: "Agents",
                newName: "AgentKey");

            migrationBuilder.AddColumn<Guid>(
                name: "AgentId",
                table: "BackUpConfigurations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Periodicity",
                table: "BackUpConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agents",
                table: "Agents",
                column: "AgentKey");

            migrationBuilder.CreateIndex(
                name: "IX_BackUpConfigurations_AgentId",
                table: "BackUpConfigurations",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BackUpConfigurations_Agents_AgentId",
                table: "BackUpConfigurations",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentKey",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackUpConfigurations_Agents_AgentId",
                table: "BackUpConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_BackUpConfigurations_AgentId",
                table: "BackUpConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agents",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "BackUpConfigurations");

            migrationBuilder.DropColumn(
                name: "Periodicity",
                table: "BackUpConfigurations");

            migrationBuilder.RenameColumn(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                newName: "BackUpSize");

            migrationBuilder.RenameColumn(
                name: "AgentKey",
                table: "Agents",
                newName: "ConnectionKey");

            migrationBuilder.AddColumn<string>(
                name: "PeriodicBackUpType",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Agents",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agents",
                table: "Agents",
                column: "Id");
        }
    }
}
