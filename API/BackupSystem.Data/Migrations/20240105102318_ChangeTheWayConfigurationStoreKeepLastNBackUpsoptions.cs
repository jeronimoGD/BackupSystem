using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTheWayConfigurationStoreKeepLastNBackUpsoptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastNBackUps",
                table: "BackUpConfigurations");

            migrationBuilder.DropColumn(
                name: "StoreLastNBackUps",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "LastNBackUpsToStore",
                table: "BackUpConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastNBackUpsToStore",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "LastNBackUps",
                table: "BackUpConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StoreLastNBackUps",
                table: "BackUpConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
