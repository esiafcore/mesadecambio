using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchemaQuotationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "quotations",
                schema: "cxc",
                newName: "quotations",
                newSchema: "fac");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4750));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4838));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4840));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4841));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4835));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4843));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(4844));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(7464));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(8003));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(8011));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(8013));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6171));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6177));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6179));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6180));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6181));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6183));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6184));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6185));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6187));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(6188));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5771));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5779));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5782));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5784));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5786));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 937, DateTimeKind.Utc).AddTicks(5788));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(2359));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 936, DateTimeKind.Utc).AddTicks(2369));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 2, 25, 37, 935, DateTimeKind.Utc).AddTicks(9815));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "quotations",
                schema: "fac",
                newName: "quotations",
                newSchema: "cxc");

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
        }
    }
}
