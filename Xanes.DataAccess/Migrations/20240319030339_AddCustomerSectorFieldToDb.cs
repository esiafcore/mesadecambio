using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerSectorFieldToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sectorid",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2238));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2271));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2274));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2277));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2263));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2283));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 834, DateTimeKind.Utc).AddTicks(1415));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(61));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(72));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(920));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(926));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(929));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(931));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(933));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(935));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(938));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(942));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(944));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(8979));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(8988));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9061));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9064));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9067));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9070));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(5880));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(5886));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3033));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3043));

            migrationBuilder.CreateIndex(
                name: "ix_customers_sectorid",
                schema: "cxc",
                table: "customers",
                column: "sectorid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers",
                column: "sectorid",
                principalSchema: "cxc",
                principalTable: "customerssectors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerssectors_sectorid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropIndex(
                name: "ix_customers_sectorid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "sectorid",
                schema: "cxc",
                table: "customers");

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
        }
    }
}
