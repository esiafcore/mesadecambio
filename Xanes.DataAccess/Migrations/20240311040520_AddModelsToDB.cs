using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddModelsToDB : Migration
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
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inactivatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    inactivatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
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
                    nameforeign = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    nameforeignsingular = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inactivatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    inactivatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customerscategories",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    isbank = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inactivatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    inactivatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customerscategories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "personstypes",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personstypes", x => x.id);
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
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotationstypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    identificationnumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    secondname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    secondsurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    businessname = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    commercialname = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    categoryid = table.Column<int>(type: "int", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    isbank = table.Column<bool>(type: "bit", nullable: false),
                    issystemrow = table.Column<bool>(type: "bit", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    createdby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    createdhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    updateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    updatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    updatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivateddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    inactivatedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    inactivatedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    inactivatedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_customerscategories_categoryid",
                        column: x => x.categoryid,
                        principalSchema: "cxc",
                        principalTable: "customerscategories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customers_personstypes_typeid",
                        column: x => x.typeid,
                        principalSchema: "cnf",
                        principalTable: "personstypes",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "bco",
                table: "banks",
                columns: new[] { "id", "bankaccountexcludeuid", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "logobank", "name", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, new Guid("9f8a706a-f0c4-4bb0-9159-d9f0af666152"), "BAC", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7271), "", "", null, null, null, null, true, "/Content/images/Bank/BacLogo.png", "Banco de America Central", null, null, null, null },
                    { 2, null, "BDF", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7294), "", "", null, null, null, null, true, "/Content/images/Bank/BdfLogo.png", "Banco de Finanza", null, null, null, null },
                    { 3, null, "LAFISE", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7295), "", "", null, null, null, null, true, "/Content/images/Bank/LafiseLogo.png", "Bancentro", null, null, null, null },
                    { 4, null, "ATLANT", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7297), "", "", null, null, null, null, true, "/Content/images/Bank/AtlantidaLogo.png", "ATLANTIDA", null, null, null, null },
                    { 5, new Guid("bbcf016d-4cdd-43b7-99b0-bea2375ce2ce"), "FICOHSA", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7290), "", "", null, null, null, null, true, "/Content/images/Bank/FicohsaLogo.png", "FICOHSA", null, null, null, null },
                    { 6, null, "BANPRO", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7299), "", "", null, null, null, null, true, "/Content/images/Bank/BanproLogo.png", "BANPRO", null, null, null, null },
                    { 7, null, "AVANZ", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7304), "", "", null, null, null, null, true, "/Content/images/Bank/AvanzLogo.png", "AVANZ", null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "currencies",
                columns: new[] { "id", "abbreviation", "code", "codeiso", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "name", "nameforeign", "nameforeignsingular", "namesingular", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "C$", "COR", "NIO", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1431), "", "", null, null, null, null, true, "CORDOBAS", "CORDOBAS", "CORDOBA", "CORDOBA", 1, null, null, null, null },
                    { 2, "U$", "USD", "USD", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1437), "", "", null, null, null, null, true, "DOLARES", "DOLLARS", "DOLLAR", "DOLAR", 2, null, null, null, null },
                    { 4, "€", "EUR", "EUR", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1440), "", "", null, null, null, null, true, "EUROS", "EUROS", "EURO", "EURO", 4, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "isbank", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "BAN", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8288), "", "", null, null, null, null, true, true, "Bancos", 1, null, null, null, null },
                    { 2, "FIN", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8294), "", "", null, null, null, null, true, true, "Financieras", 2, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 3, "IND", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8296), "", "", null, null, null, null, true, "Industrias", 3, null, null, null, null },
                    { 4, "ONG", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8297), "", "", null, null, null, null, true, "ONG", 4, null, null, null, null },
                    { 5, "UNI", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8299), "", "", null, null, null, null, true, "Universidades", 5, null, null, null, null },
                    { 6, "COM", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8300), "", "", null, null, null, null, true, "Comercial", 6, null, null, null, null },
                    { 7, "FAM", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8302), "", "", null, null, null, null, true, "Farmacias", 7, null, null, null, null },
                    { 8, "TEC", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8303), "", "", null, null, null, null, true, "Tecnológicos", 8, null, null, null, null },
                    { 9, "OTR", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8305), "", "", null, null, null, null, true, "Otros", 9, null, null, null, null },
                    { 10, "SER", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8306), "", "", null, null, null, null, true, "Servicios", 10, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "personstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "NAT", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(5894), "", "", "Natural", 1, null, null, null, null },
                    { 2, "JUR", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(5902), "", "", "Jurídico", 2, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "fac",
                table: "quotationstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "name", "numeral", "ordersequence", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "COM", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3574), "", "", "COMPRA", 1, (short)10, null, null, null, null },
                    { 2, "VTA", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3580), "", "", "VENTA", 2, (short)20, null, null, null, null },
                    { 3, "TRF", 1, "", new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3582), "", "", "TRANSFERENCIA", 4, (short)30, null, null, null, null }
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
                name: "ix_customers_categoryid",
                schema: "cxc",
                table: "customers",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_code",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_identificationnumber",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "identificationnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_typeid",
                schema: "cxc",
                table: "customers",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "ix_customerscategories_companyid_code",
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customerscategories_companyid_numeral",
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "companyid", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_personstypes_companyid_code",
                schema: "cnf",
                table: "personstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_personstypes_companyid_numeral",
                schema: "cnf",
                table: "personstypes",
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
                name: "customers",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "quotationstypes",
                schema: "fac");

            migrationBuilder.DropTable(
                name: "customerscategories",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "personstypes",
                schema: "cnf");
        }
    }
}
