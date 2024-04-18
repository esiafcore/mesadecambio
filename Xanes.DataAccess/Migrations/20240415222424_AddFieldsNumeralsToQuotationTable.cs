using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsNumeralsToQuotationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "currencyoriginexchangetype",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddColumn<short>(
                name: "currencytransatype",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddColumn<int>(
                name: "typenumeral",
                schema: "fac",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8178));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8196));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8198));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8200));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8192));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8202));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 7, DateTimeKind.Utc).AddTicks(8204));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(4749));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(2048));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(2056));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(2059));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1008));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1016));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1018));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1020));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1021));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1023));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1025));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1026));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1028));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 9, DateTimeKind.Utc).AddTicks(1030));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(2998));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(3006));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(3009));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(3014));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 10, DateTimeKind.Utc).AddTicks(3016));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(6813));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(6818));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(4335));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(4342));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 15, 22, 24, 23, 8, DateTimeKind.Utc).AddTicks(4344));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currencyoriginexchangetype",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "currencytransatype",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "typenumeral",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5373));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5400));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5401));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5403));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5405));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(5408));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(7624));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(8743));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(8750));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 838, DateTimeKind.Utc).AddTicks(8753));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6001));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6005));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6007));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6008));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6010));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6011));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6028));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6030));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6032));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(6034));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6009));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6099));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6103));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6105));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6107));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 840, DateTimeKind.Utc).AddTicks(6109));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(2637));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(2640));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(635));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(640));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 13, 2, 32, 39, 839, DateTimeKind.Utc).AddTicks(642));
        }
    }
}
