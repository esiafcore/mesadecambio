using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicVirtualByDefaultToRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_currenciesexchangerates_currencies_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_personstypes_typeid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5061));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5063));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5066));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5056));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5068));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 461, DateTimeKind.Utc).AddTicks(5071));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(8955));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 462, DateTimeKind.Utc).AddTicks(6706));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 462, DateTimeKind.Utc).AddTicks(6716));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 462, DateTimeKind.Utc).AddTicks(6721));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3950));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3966));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3969));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3971));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3973));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3975));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3978));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3980));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3982));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 464, DateTimeKind.Utc).AddTicks(3984));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5834));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5849));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5853));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5857));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5860));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 466, DateTimeKind.Utc).AddTicks(5863));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 463, DateTimeKind.Utc).AddTicks(5799));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 463, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 463, DateTimeKind.Utc).AddTicks(1251));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 463, DateTimeKind.Utc).AddTicks(1262));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 8, 57, 463, DateTimeKind.Utc).AddTicks(1265));

            migrationBuilder.AddForeignKey(
                name: "fk_currenciesexchangerates_currencies_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates",
                column: "currencyid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers",
                column: "categoryid",
                principalSchema: "cxc",
                principalTable: "customerscategories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers",
                column: "sectorid",
                principalSchema: "cxc",
                principalTable: "customerssectors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_personstypes_typeid",
                schema: "cxc",
                table: "customers",
                column: "typeid",
                principalSchema: "cnf",
                principalTable: "personstypes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_currenciesexchangerates_currencies_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_customers_personstypes_typeid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2022));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2042));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2044));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2046));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2038));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2052));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2054));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(6305));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6630));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6638));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6641));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6863));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6869));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6873));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6875));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6877));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6878));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6880));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6882));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6884));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4220));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4227));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4231));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4233));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4236));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4238));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(1881));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(1886));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9157));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9172));

            migrationBuilder.AddForeignKey(
                name: "fk_currenciesexchangerates_currencies_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates",
                column: "currencyid",
                principalSchema: "cnf",
                principalTable: "currencies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers",
                column: "categoryid",
                principalSchema: "cxc",
                principalTable: "customerscategories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers",
                column: "sectorid",
                principalSchema: "cxc",
                principalTable: "customerssectors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_customers_personstypes_typeid",
                schema: "cxc",
                table: "customers",
                column: "typeid",
                principalSchema: "cnf",
                principalTable: "personstypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
