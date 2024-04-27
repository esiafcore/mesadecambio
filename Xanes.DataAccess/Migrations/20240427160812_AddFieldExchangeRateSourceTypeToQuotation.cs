using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldExchangeRateSourceTypeToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "exchangeratesourcetype",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 27, 16, 8, 10, 892, DateTimeKind.Utc).AddTicks(4275));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchangeratesourcetype",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 26, 17, 53, 44, 390, DateTimeKind.Utc).AddTicks(694));
        }
    }
}
