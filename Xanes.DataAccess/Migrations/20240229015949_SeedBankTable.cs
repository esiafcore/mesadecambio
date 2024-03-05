using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations;

/// <inheritdoc />
public partial class SeedBankTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            schema: "bco",
            table: "banks",
            columns: new[] { "id", "bankaccountexcludeuid", "code", "comisionbancariaporcentaje", "companyid", "iscompany", "logobank", "name", "orderpriority" },
            values: new object[,]
            {
                { 1, new Guid("234f2ad8-2a98-e911-b070-4ccc6a8ad00b"), "BAC", 0m, 1, false, "/Content/images/Bank/BacLogo.png", "Banco de America Central", 0 },
                { 2, null, "BDF", 0m, 1, false, "/Content/images/Bank/BdfLogo.png", "Banco de Finanza", 0 },
                { 3, null, "LAFISE", 0m, 1, false, "/Content/images/Bank/LafiseLogo.png", "Bancentro", 0 },
                { 4, null, "ATLANT", 0m, 1, false, "/Content/images/Bank/AtlantidaLogo.png", "ATLANTIDA", 0 },
                { 5, new Guid("530e22a8-2c98-e911-b070-4ccc6a8ad00b"), "FICOHSA", 0m, 1, false, "/Content/images/Bank/FicohsaLogo.png", "FICOHSA", 0 },
                { 6, null, "BANPRO", 0m, 1, false, "/Content/images/Bank/BanproLogo.png", "BANPRO", 0 },
                { 7, null, "AVANZ", 0m, 1, false, "/Content/images/Bank/AvanzLogo.png", "AVANZ", 0 }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 5);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 6);

        migrationBuilder.DeleteData(
            schema: "bco",
            table: "banks",
            keyColumn: "id",
            keyValue: 7);
    }
}
