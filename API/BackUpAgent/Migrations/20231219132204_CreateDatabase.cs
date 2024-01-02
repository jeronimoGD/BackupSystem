using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackUpAgent.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackUpConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigurationName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TarjetDbName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriodicBackUpType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CleanEventTables = table.Column<bool>(type: "bit", nullable: false),
                    CreateCloudBackUp = table.Column<bool>(type: "bit", nullable: false),
                    StoreLastNBackUps = table.Column<bool>(type: "bit", nullable: false),
                    LastNBackUps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUpConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackUpHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackUpName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BackUpSizeInMB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuckUpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableToDownload = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUpHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackUpConfigurations_ConfigurationName",
                table: "BackUpConfigurations",
                column: "ConfigurationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackUpHistory_BackUpName",
                table: "BackUpHistory",
                column: "BackUpName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackUpConfigurations");

            migrationBuilder.DropTable(
                name: "BackUpHistory");
        }
    }
}
