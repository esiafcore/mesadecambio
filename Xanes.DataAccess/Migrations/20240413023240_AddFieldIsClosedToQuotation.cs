using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldIsClosedToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isclosed",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isclosed",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1011));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1051));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1055));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1058));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1044));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1061));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 870, DateTimeKind.Utc).AddTicks(1151));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 888, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 873, DateTimeKind.Utc).AddTicks(1994));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 873, DateTimeKind.Utc).AddTicks(2006));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 873, DateTimeKind.Utc).AddTicks(2011));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7100));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7115));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7119));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7125));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7136));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 878, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6087));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6106));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6112));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6116));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6132));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 886, DateTimeKind.Utc).AddTicks(6137));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 876, DateTimeKind.Utc).AddTicks(6145));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 876, DateTimeKind.Utc).AddTicks(6158));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 874, DateTimeKind.Utc).AddTicks(8060));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 874, DateTimeKind.Utc).AddTicks(8072));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 11, 18, 59, 42, 874, DateTimeKind.Utc).AddTicks(8075));
        }
    }
}
