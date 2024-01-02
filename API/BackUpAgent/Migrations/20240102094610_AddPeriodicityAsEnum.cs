using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackUpAgent.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodicityAsEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodicBackUpType",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<int>(
                name: "Periodicity",
                table: "BackUpConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periodicity",
                table: "BackUpConfigurations");

            migrationBuilder.AddColumn<string>(
                name: "PeriodicBackUpType",
                table: "BackUpConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
