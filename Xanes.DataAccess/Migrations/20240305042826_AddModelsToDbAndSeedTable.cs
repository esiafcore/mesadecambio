using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddModelsToDbAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bco");

            migrationBuilder.EnsureSchema(
                name: "cnf");

            migrationBuilder.EnsureSchema(
                name: "cxc");

            migrationBuilder.EnsureSchema(
                name: "fac");

            migrationBuilder.CreateTable(
                name: "banks",
                schema: "bco",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    bankingcommissionpercentage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    bankaccountexcludeuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    iscompany = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    orderpriority = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    logobank = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeiso = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    abbreviation = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    namesingular = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    namefor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    nameforsingular = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customerstypes",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customerstypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quotationstypes",
                schema: "fac",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ordersequence = table.Column<short>(type: "smallint", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotationstypes", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "bco",
                table: "banks",
                columns: new[] { "id", "bankaccountexcludeuid", "code", "companyid", "logobank", "name" },
                values: new object[,]
                {
                    { 1, new Guid("9f8a706a-f0c4-4bb0-9159-d9f0af666152"), "BAC", 1, "/Content/images/Bank/BacLogo.png", "Banco de America Central" },
                    { 2, null, "BDF", 1, "/Content/images/Bank/BdfLogo.png", "Banco de Finanza" },
                    { 3, null, "LAFISE", 1, "/Content/images/Bank/LafiseLogo.png", "Bancentro" },
                    { 4, null, "ATLANT", 1, "/Content/images/Bank/AtlantidaLogo.png", "ATLANTIDA" },
                    { 5, new Guid("bbcf016d-4cdd-43b7-99b0-bea2375ce2ce"), "FICOHSA", 1, "/Content/images/Bank/FicohsaLogo.png", "FICOHSA" },
                    { 6, null, "BANPRO", 1, "/Content/images/Bank/BanproLogo.png", "BANPRO" },
                    { 7, null, "AVANZ", 1, "/Content/images/Bank/AvanzLogo.png", "AVANZ" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_banks_companyid_code",
                schema: "bco",
                table: "banks",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_abbreviation",
                schema: "cnf",
                table: "currencies",
                columns: new[] { "companyid", "abbreviation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_code",
                schema: "cnf",
                table: "currencies",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_codeiso",
                schema: "cnf",
                table: "currencies",
                columns: new[] { "companyid", "codeiso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_numeral",
                schema: "cnf",
                table: "currencies",
                columns: new[] { "companyid", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customerstypes_companyid_code",
                schema: "cxc",
                table: "customerstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customerstypes_companyid_numeral",
                schema: "cxc",
                table: "customerstypes",
                columns: new[] { "companyid", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotationstypes_companyid_code",
                schema: "fac",
                table: "quotationstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotationstypes_companyid_numeral",
                schema: "fac",
                table: "quotationstypes",
                columns: new[] { "companyid", "numeral" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banks",
                schema: "bco");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "cnf");

            migrationBuilder.DropTable(
                name: "customerstypes",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "quotationstypes",
                schema: "fac");
        }
    }
}
