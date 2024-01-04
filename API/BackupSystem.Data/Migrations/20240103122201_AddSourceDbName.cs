using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceDbName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "SourceDbName",
                table: "BackUpConfigurations");
        }
    }
}
