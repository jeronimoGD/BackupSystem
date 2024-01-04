using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackUpAgent.Migrations
{
    /// <inheritdoc />
    public partial class AddPathDescriptionAndSuccesfulFlagToBackUpHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AlterColumn<string>(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
