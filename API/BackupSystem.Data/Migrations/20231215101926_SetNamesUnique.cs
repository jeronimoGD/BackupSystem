using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetNamesUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BackUpName",
                table: "BackUpHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationName",
                table: "BackUpConfigurations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AgentName",
                table: "Agents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_BackUpHistory_BackUpName",
                table: "BackUpHistory",
                column: "BackUpName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackUpConfigurations_ConfigurationName",
                table: "BackUpConfigurations",
                column: "ConfigurationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_AgentName",
                table: "Agents",
                column: "AgentName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BackUpHistory_BackUpName",
                table: "BackUpHistory");

            migrationBuilder.DropIndex(
                name: "IX_BackUpConfigurations_ConfigurationName",
                table: "BackUpConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_Agents_AgentName",
                table: "Agents");

            migrationBuilder.AlterColumn<string>(
                name: "BackUpName",
                table: "BackUpHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ConfigurationName",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AgentName",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
