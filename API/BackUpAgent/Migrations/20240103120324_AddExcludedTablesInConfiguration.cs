using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackUpAgent.Migrations
{
    /// <inheritdoc />
    public partial class AddExcludedTablesInConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleanEventTables",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "ExcludedTablesJsonList",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceDbName",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcludedTablesJsonList",
                table: "BackUpConfigurations");

            migrationBuilder.DropColumn(
                name: "SourceDbName",
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
