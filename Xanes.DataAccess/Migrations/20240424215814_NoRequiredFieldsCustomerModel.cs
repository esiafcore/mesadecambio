using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NoRequiredFieldsCustomerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "secondsurname",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "secondname",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 24, 21, 58, 13, 205, DateTimeKind.Utc).AddTicks(2195));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "secondsurname",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "secondname",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 24, 17, 48, 50, 48, DateTimeKind.Utc).AddTicks(8886));
        }
    }
}
