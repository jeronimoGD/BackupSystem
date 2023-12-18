using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackupSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackUpConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigurationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TarjetDbName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PeriodicBackUpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    BackUpName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackUpSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuckUpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableToDownload = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackUpHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "BackUpConfigurations");

            migrationBuilder.DropTable(
                name: "BackUpHistory");
        }
    }
}
