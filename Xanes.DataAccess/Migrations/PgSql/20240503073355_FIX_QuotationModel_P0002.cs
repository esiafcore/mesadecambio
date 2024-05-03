using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_QuotationModel_P0002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_quotations_currencies_currencytransferid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.AddColumn<int>(
                name: "bankaccountsourceid",
                schema: "fac",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "bankaccounttargetid",
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
                value: new DateTime(2024, 5, 3, 7, 33, 53, 700, DateTimeKind.Utc).AddTicks(9189));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_bankaccountsourceid",
                schema: "fac",
                table: "quotations",
                column: "bankaccountsourceid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_bankaccounttargetid",
                schema: "fac",
                table: "quotations",
                column: "bankaccounttargetid");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_banksaccounts_bankaccountsourceid",
                schema: "fac",
                table: "quotations",
                column: "bankaccountsourceid",
                principalSchema: "bco",
                principalTable: "banksaccounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_banksaccounts_bankaccounttargetid",
                schema: "fac",
                table: "quotations",
                column: "bankaccounttargetid",
                principalSchema: "bco",
                principalTable: "banksaccounts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_currencies_currencytransferid",
                schema: "fac",
                table: "quotations",
                column: "currencytransferid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_quotations_banksaccounts_bankaccountsourceid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropForeignKey(
                name: "fk_quotations_banksaccounts_bankaccounttargetid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropForeignKey(
                name: "fk_quotations_currencies_currencytransferid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "ix_quotations_bankaccountsourceid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropIndex(
                name: "ix_quotations_bankaccounttargetid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "bankaccountsourceid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "bankaccounttargetid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 2, 22, 22, 54, 801, DateTimeKind.Utc).AddTicks(4391));

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
    }
}
