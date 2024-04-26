using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsCurrencyTransfertoModelQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_quotations_currencies_currencyoriginexchangeid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "ix_quotations_currencyoriginexchangeid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.RenameColumn(
                name: "currencyoriginexchangetype",
                schema: "fac",
                table: "quotations",
                newName: "currencydeposittype");

            migrationBuilder.RenameColumn(
                name: "currencyoriginexchangeid",
                schema: "fac",
                table: "quotations",
                newName: "currencytransfertype");

            migrationBuilder.AddColumn<int>(
                name: "currencydepositid",
                schema: "fac",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "currencytransferid",
                schema: "fac",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 10, 15, 206, DateTimeKind.Utc).AddTicks(8554));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencydepositid",
                schema: "fac",
                table: "quotations",
                column: "currencydepositid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencytransferid",
                schema: "fac",
                table: "quotations",
                column: "currencytransferid");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_currencies_currencydepositid",
                schema: "fac",
                table: "quotations",
                column: "currencydepositid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_currencies_currencytransferid",
                schema: "fac",
                table: "quotations",
                column: "currencytransferid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_quotations_currencies_currencydepositid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropForeignKey(
                name: "fk_quotations_currencies_currencytransferid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "ix_quotations_currencydepositid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "ix_quotations_currencytransferid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "currencydepositid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "currencytransferid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.RenameColumn(
                name: "currencytransfertype",
                schema: "fac",
                table: "quotations",
                newName: "currencyoriginexchangeid");

            migrationBuilder.RenameColumn(
                name: "currencydeposittype",
                schema: "fac",
                table: "quotations",
                newName: "currencyoriginexchangetype");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 24, 21, 58, 13, 205, DateTimeKind.Utc).AddTicks(2195));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencyoriginexchangeid",
                schema: "fac",
                table: "quotations",
                column: "currencyoriginexchangeid");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_currencies_currencyoriginexchangeid",
                schema: "fac",
                table: "quotations",
                column: "currencyoriginexchangeid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id");
        }
    }
}
