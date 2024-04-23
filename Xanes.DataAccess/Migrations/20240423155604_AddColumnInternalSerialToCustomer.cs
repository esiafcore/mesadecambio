using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInternalSerialToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customers_companyid_code",
                schema: "cxc",
                table: "customers");

            migrationBuilder.AddColumn<string>(
                name: "internalserial",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "Z");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 23, 15, 56, 2, 878, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_internalserial_code",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "internalserial", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customers_companyid_internalserial_code",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "internalserial",
                schema: "cxc",
                table: "customers");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 23, 0, 38, 38, 43, DateTimeKind.Utc).AddTicks(1192));

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_code",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "code" },
                unique: true);
        }
    }
}
