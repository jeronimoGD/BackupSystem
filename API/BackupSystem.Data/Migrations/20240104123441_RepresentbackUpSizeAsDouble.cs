using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RepresentbackUpSizeAsDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
