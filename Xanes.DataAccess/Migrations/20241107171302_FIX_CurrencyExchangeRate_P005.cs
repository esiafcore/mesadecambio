using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FIX_CurrencyExchangeRate_P005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "buyrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "officialrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "sellrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates",
                type: "decimal(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "buyrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates");

            migrationBuilder.DropColumn(
                name: "officialrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates");

            migrationBuilder.DropColumn(
                name: "sellrateorigin",
                schema: "cnf",
                table: "currenciesexchangerates");
        }
    }
}
