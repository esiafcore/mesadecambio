using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenamePersonTypeIdByNumeral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "persontypeid",
                schema: "cnf",
                table: "identificationstypes",
                newName: "numeral");

            migrationBuilder.RenameIndex(
                name: "ix_identificationstypes_companyid_persontypeid",
                schema: "cnf",
                table: "identificationstypes",
                newName: "ix_identificationstypes_companyid_numeral");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8630));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8651));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8653));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8655));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8647));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8657));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8659));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2540));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2547));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2549));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9234));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9236));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9238));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9239));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9241));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9242));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9244));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9245));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9247));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7966));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7973));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7976));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7978));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7981));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7983));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(6964));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(6968));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4649));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4655));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4657));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numeral",
                schema: "cnf",
                table: "identificationstypes",
                newName: "persontypeid");

            migrationBuilder.RenameIndex(
                name: "ix_identificationstypes_companyid_numeral",
                schema: "cnf",
                table: "identificationstypes",
                newName: "ix_identificationstypes_companyid_persontypeid");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2780));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2782));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2785));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2776));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2787));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2789));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5274));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5280));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5283));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5284));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5286));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5288));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5289));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5344));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5346));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5348));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7321));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7331));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7334));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7336));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7339));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7341));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(2519));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(2524));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9818));
        }
    }
}
