using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrenciesExchangeRatesModelToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "currenciesexchangerates",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currencytype = table.Column<int>(type: "int", nullable: false),
                    currencyid = table.Column<int>(type: "int", nullable: false),
                    datetransa = table.Column<DateOnly>(type: "date", nullable: false),
                    sellrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    buyrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    officialrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currenciesexchangerates", x => x.id);
                    table.ForeignKey(
                        name: "fk_currenciesexchangerates_currencies_currencyid",
                        column: x => x.currencyid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8818));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8852));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8896));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8898));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8847));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8900));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 605, DateTimeKind.Utc).AddTicks(8902));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 608, DateTimeKind.Utc).AddTicks(222));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(4287));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(4294));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(4297));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2630));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2635));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2637));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2639));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2640));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2642));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2643));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2645));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2646));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(2648));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8453));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8466));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8469));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8471));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8473));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 607, DateTimeKind.Utc).AddTicks(8476));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(8535));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(8541));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(6424));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(6430));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 2, 17, 3, 606, DateTimeKind.Utc).AddTicks(6432));

            migrationBuilder.CreateIndex(
                name: "ix_currenciesexchangerates_companyid_currencyid_datetransa",
                schema: "cnf",
                table: "currenciesexchangerates",
                columns: new[] { "companyid", "currencyid", "datetransa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currenciesexchangerates_companyid_currencytype_datetransa",
                schema: "cnf",
                table: "currenciesexchangerates",
                columns: new[] { "companyid", "currencytype", "datetransa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currenciesexchangerates_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates",
                column: "currencyid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currenciesexchangerates",
                schema: "cnf");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6391));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6410));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6412));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6414));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6406));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6455));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 285, DateTimeKind.Utc).AddTicks(6457));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(6110));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(143));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(148));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(151));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8742));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8746));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8749));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8750));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8751));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8753));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8755));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8756));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8758));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(8759));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4488));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4495));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4498));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4500));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4503));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 287, DateTimeKind.Utc).AddTicks(4505));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(4475));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(2299));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 17, 19, 28, 35, 286, DateTimeKind.Utc).AddTicks(2308));
        }
    }
}
