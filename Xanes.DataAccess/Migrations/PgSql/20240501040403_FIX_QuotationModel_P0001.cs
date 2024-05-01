using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_QuotationModel_P0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "closedby",
                schema: "fac",
                table: "quotations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "closeddate",
                schema: "fac",
                table: "quotations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "closedhostname",
                schema: "fac",
                table: "quotations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "closedipv4",
                schema: "fac",
                table: "quotations",
                type: "nvarchar(75)",
                maxLength: 75,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "totaldeposit",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "totaltransfer",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 1, 4, 4, 1, 181, DateTimeKind.Utc).AddTicks(6425));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "closedby",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "closeddate",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "closedhostname",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "closedipv4",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "totaldeposit",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "totaltransfer",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 27, 16, 8, 10, 892, DateTimeKind.Utc).AddTicks(4275));
        }
    }
}
