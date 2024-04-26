using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldAmountExchangetoModelQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amounttransa",
                schema: "fac",
                table: "quotations",
                newName: "amounttransaction");

            migrationBuilder.AddColumn<decimal>(
                name: "amountexchange",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 52, 13, 287, DateTimeKind.Utc).AddTicks(3222));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountexchange",
                schema: "fac",
                table: "quotations");

            migrationBuilder.RenameColumn(
                name: "amounttransaction",
                schema: "fac",
                table: "quotations",
                newName: "amounttransa");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 10, 15, 206, DateTimeKind.Utc).AddTicks(8554));
        }
    }
}
