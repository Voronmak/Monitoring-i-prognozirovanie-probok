using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Мониторинг_и_прогнозирование_пробок.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrognozSpeeds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    PrognozaDataVremya = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataSozdaniaPrognoza = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrognozSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrognozSpeeds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HistorySpeeds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    DataVremya = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SrScorost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScorostMedlPotoka = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScorostBystrPotoka = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IDPrognoza = table.Column<int>(type: "int", nullable: false),
                    Прогноз_скоростиID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorySpeeds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HistorySpeeds_PrognozSpeeds_Прогноз_скоростиID",
                        column: x => x.Прогноз_скоростиID,
                        principalTable: "PrognozSpeeds",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TypeDays",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prazdnik = table.Column<bool>(type: "bit", nullable: false),
                    Nagruzki = table.Column<int>(type: "int", nullable: false),
                    IDPrognoz = table.Column<int>(type: "int", nullable: false),
                    Прогноз_СкоростиID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDays", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TypeDays_PrognozSpeeds_Прогноз_СкоростиID",
                        column: x => x.Прогноз_СкоростиID,
                        principalTable: "PrognozSpeeds",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorySpeeds_Прогноз_скоростиID",
                table: "HistorySpeeds",
                column: "Прогноз_скоростиID");

            migrationBuilder.CreateIndex(
                name: "IX_TypeDays_Прогноз_СкоростиID",
                table: "TypeDays",
                column: "Прогноз_СкоростиID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorySpeeds");

            migrationBuilder.DropTable(
                name: "TypeDays");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PrognozSpeeds");
        }
    }
}
