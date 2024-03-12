using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentificationTypeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "substitutionexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "regularexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "formatexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2543));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2572));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2574));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2568));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2578));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(2580));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(8780));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(8791));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 784, DateTimeKind.Utc).AddTicks(8795));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5805,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(1586));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5806,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(1582));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5807,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(1578));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5808,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(1574));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5809,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(1557));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(781));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(793));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(796));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(798));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(800));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(802));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(804));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(805));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(807));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 786, DateTimeKind.Utc).AddTicks(809));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5287));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5297));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5301));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5304));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5306));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 787, DateTimeKind.Utc).AddTicks(5309));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 785, DateTimeKind.Utc).AddTicks(6979));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 785, DateTimeKind.Utc).AddTicks(6986));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 785, DateTimeKind.Utc).AddTicks(3032));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 785, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 18, 20, 43, 785, DateTimeKind.Utc).AddTicks(3043));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "substitutionexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "regularexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "formatexpressionnumber",
                schema: "cnf",
                table: "identificationstypes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2164));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2167));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2169));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2171));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2173));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5867));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5875));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5878));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5805,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8815));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5806,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8812));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5807,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8809));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5808,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5809,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8789));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2561));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2566));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2568));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2570));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2572));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2573));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2575));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2578));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2580));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1292));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1298));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1304));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1306));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(239));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(243));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7946));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7952));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7953));
        }
    }
}
