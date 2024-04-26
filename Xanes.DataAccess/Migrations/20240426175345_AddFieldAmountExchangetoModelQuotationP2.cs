using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldAmountExchangetoModelQuotationP2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "amountexchange",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 53, 44, 390, DateTimeKind.Utc).AddTicks(694));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "amountexchange",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 52, 13, 287, DateTimeKind.Utc).AddTicks(3222));
        }
    }
}
