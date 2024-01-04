using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackUpAgent.Migrations
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
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BackUpSizeInMB",
                table: "BackUpHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
