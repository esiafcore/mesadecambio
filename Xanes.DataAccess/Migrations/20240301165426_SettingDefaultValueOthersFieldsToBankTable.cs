using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
using Xanes.DataAccess.Data;

namespace Xanes.DataAccess.Migrations;

/// <inheritdoc />
public partial class SettingDefaultValueOthersFieldsToBankTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "orderpriority",
            schema: "bco",
            table: "banks",
            type: "int",
            nullable: false,
            defaultValue: 1,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<bool>(
            name: "iscompany",
            schema: "bco",
            table: "banks",
            type: "bit",
            nullable: false,
            defaultValue: false,
            oldClrType: typeof(bool),
            oldType: "bit");

        migrationBuilder.AlterColumn<decimal>(
            name: "bankingcommissionpercentage",
            schema: "bco",
            table: "banks",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 1,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 2,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 3,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 4,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 5,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 6,
            column: "orderpriority",
            value: 1);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 7,
            column: "orderpriority",
            value: 1);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "orderpriority",
            schema: "bco",
            table: "banks",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int",
            oldDefaultValue: 1);

        migrationBuilder.AlterColumn<bool>(
            name: "iscompany",
            schema: "bco",
            table: "banks",
            type: "bit",
            nullable: false,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldDefaultValue: false);

        migrationBuilder.AlterColumn<decimal>(
            name: "bankingcommissionpercentage",
            schema: "bco",
            table: "banks",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldDefaultValue: 0m);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 1,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 2,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 3,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 4,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 5,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 6,
            column: "orderpriority",
            value: 0);

        migrationBuilder.UpdateData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 7,
            column: "orderpriority",
            value: 0);
    }
}
