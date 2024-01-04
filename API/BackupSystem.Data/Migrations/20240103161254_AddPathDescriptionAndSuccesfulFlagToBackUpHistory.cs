using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPathDescriptionAndSuccesfulFlagToBackUpHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackUpPath",
                table: "BackUpHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BackUpHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessfull",
                table: "BackUpHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackUpPath",
                table: "BackUpHistory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BackUpHistory");

            migrationBuilder.DropColumn(
                name: "IsSuccessfull",
                table: "BackUpHistory");
        }
    }
}
