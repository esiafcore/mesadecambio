using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class AddFieldLedgerAccountIdToBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ledgeraccountid",
                schema: "bco",
                table: "banksaccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "literalprefix",
                schema: "bco",
                table: "banksaccounts",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 2, 22, 22, 54, 801, DateTimeKind.Utc).AddTicks(4391));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ledgeraccountid",
                schema: "bco",
                table: "banksaccounts");

            migrationBuilder.DropColumn(
                name: "literalprefix",
                schema: "bco",
                table: "banksaccounts");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 2, 22, 16, 11, 369, DateTimeKind.Utc).AddTicks(5510));
        }
    }
}
