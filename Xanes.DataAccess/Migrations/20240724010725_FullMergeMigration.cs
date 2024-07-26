using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FullMergeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "bco");

            migrationBuilder.EnsureSchema(
                name: "cxc");

            migrationBuilder.EnsureSchema(
                name: "cnf");

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
                    logourl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    logolocalpath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                name: "businessexecutives",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    secondname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    secondsurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ispayment = table.Column<bool>(type: "bit", nullable: false),
                    isloan = table.Column<bool>(type: "bit", nullable: false),
                    isdefault = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("pk_businessexecutives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identificationnumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    commercialname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    businessname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    countrycode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    countrynumber = table.Column<short>(type: "smallint", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    billingauthorizationnumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    journalauthorizationnumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    addressprimary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    website = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    imagesplashurl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    imagesplashlocalpath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    imagelogourl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    imagelogolocalpath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    usebranch = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("pk_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configscxc",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isautomaticallycustomercode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    sequentialnumbercustomer = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    sequentialnumberdraftcustomer = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
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
                    table.PrimaryKey("pk_configscxc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configsfac",
                schema: "fac",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isautomaticallyquotationcode = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    sequentialnumberquotation = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    sequentialnumberdraftquotation = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
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
                    table.PrimaryKey("pk_configsfac", x => x.id);
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
                name: "customerssectors",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    codepath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    idpath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    depthnumber = table.Column<short>(type: "smallint", nullable: false),
                    parentid = table.Column<int>(type: "int", nullable: true),
                    typelevel = table.Column<short>(type: "smallint", nullable: false),
                    sequentialnumber = table.Column<short>(type: "smallint", nullable: true),
                    sequentialdraftnumber = table.Column<short>(type: "smallint", nullable: true),
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
                    table.PrimaryKey("pk_customerssectors", x => x.id);
                    table.ForeignKey(
                        name: "fk_customerssectors_customerssectors_parentid",
                        column: x => x.parentid,
                        principalSchema: "cxc",
                        principalTable: "customerssectors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "identificationstypes",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    islegal = table.Column<bool>(type: "bit", nullable: false),
                    isforeign = table.Column<bool>(type: "bit", nullable: false),
                    regularexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    formatexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    substitutionexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    identificationmaxlength = table.Column<short>(type: "smallint", nullable: false),
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
                    table.PrimaryKey("pk_identificationstypes", x => x.id);
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
                name: "banksaccounts",
                schema: "bco",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parentid = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    islocal = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    currencyid = table.Column<int>(type: "int", nullable: false),
                    currencytype = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    literalprefix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ledgeraccountid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("pk_banksaccounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_banksaccounts_banks_parentid",
                        column: x => x.parentid,
                        principalSchema: "bco",
                        principalTable: "banks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_banksaccounts_currencies_currencyid",
                        column: x => x.currencyid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "currenciesexchangerates",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currencytype = table.Column<int>(type: "int", nullable: false),
                    currencyid = table.Column<int>(type: "int", nullable: false),
                    datetransa = table.Column<DateOnly>(type: "date", nullable: false),
                    sellrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    buyrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    officialrate = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
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
                    table.PrimaryKey("pk_currenciesexchangerates", x => x.id);
                    table.ForeignKey(
                        name: "fk_currenciesexchangerates_currencies_currencyid",
                        column: x => x.currencyid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identificationtypeid = table.Column<int>(type: "int", nullable: false),
                    identificationtypenumber = table.Column<int>(type: "int", nullable: false),
                    identificationtypecode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    typenumeral = table.Column<int>(type: "int", nullable: false),
                    sectorid = table.Column<int>(type: "int", nullable: false),
                    internalserial = table.Column<string>(type: "nvarchar(1)", nullable: false, defaultValue: "Z"),
                    code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    identificationnumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    secondname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    secondsurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    businessname = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    commercialname = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    addressprimary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                        name: "fk_customers_customerssectors_sectorid",
                        column: x => x.sectorid,
                        principalSchema: "cxc",
                        principalTable: "customerssectors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customers_identificationstypes_identificationtypeid",
                        column: x => x.identificationtypeid,
                        principalSchema: "cnf",
                        principalTable: "identificationstypes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customers_personstypes_typeid",
                        column: x => x.typeid,
                        principalSchema: "cnf",
                        principalTable: "personstypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quotations",
                schema: "fac",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datetransa = table.Column<DateOnly>(type: "date", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    typenumeral = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    internalserial = table.Column<string>(type: "nvarchar(1)", nullable: false, defaultValue: "Z"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    customerid = table.Column<int>(type: "int", nullable: false),
                    bankaccountsourceid = table.Column<int>(type: "int", nullable: true),
                    bankaccounttargetid = table.Column<int>(type: "int", nullable: true),
                    currencydepositid = table.Column<int>(type: "int", nullable: false),
                    currencydeposittype = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    currencytransferid = table.Column<int>(type: "int", nullable: false),
                    currencytransfertype = table.Column<int>(type: "int", nullable: false),
                    currencytransaid = table.Column<int>(type: "int", nullable: false),
                    currencytransatype = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    exchangerateofficialtransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratebuytransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangerateselltransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratesourcetype = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    exchangerateofficialreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratebuyreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratesellreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    amounttransaction = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    amountcommission = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    amountexchange = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    amountrevenue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    amountcost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    totaldeposit = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    totaltransfer = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    isposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isclosed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isloan = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isvoid = table.Column<bool>(type: "bit", nullable: false),
                    ispayment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isadjustment = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    isbank = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    businessexecutiveid = table.Column<int>(type: "int", nullable: false),
                    businessexecutivecode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    closeddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    closedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    closedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    closedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    recloseddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reclosedby = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    reclosedipv4 = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    reclosedhostname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("pk_quotations", x => x.id);
                    table.ForeignKey(
                        name: "fk_quotations_banksaccounts_bankaccountsourceid",
                        column: x => x.bankaccountsourceid,
                        principalSchema: "bco",
                        principalTable: "banksaccounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_banksaccounts_bankaccounttargetid",
                        column: x => x.bankaccounttargetid,
                        principalSchema: "bco",
                        principalTable: "banksaccounts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_businessexecutives_businessexecutiveid",
                        column: x => x.businessexecutiveid,
                        principalSchema: "cxc",
                        principalTable: "businessexecutives",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_currencies_currencydepositid",
                        column: x => x.currencydepositid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_currencies_currencytransaid",
                        column: x => x.currencytransaid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_currencies_currencytransferid",
                        column: x => x.currencytransferid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_customers_customerid",
                        column: x => x.customerid,
                        principalSchema: "cxc",
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotations_quotationstypes_typeid",
                        column: x => x.typeid,
                        principalSchema: "fac",
                        principalTable: "quotationstypes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quotationsdetails",
                schema: "fac",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parentid = table.Column<int>(type: "int", nullable: false),
                    quotationdetailtype = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    linenumber = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    currencydetailid = table.Column<int>(type: "int", nullable: false),
                    banksourceid = table.Column<int>(type: "int", nullable: false),
                    banktargetid = table.Column<int>(type: "int", nullable: false),
                    amountdetail = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    isjournalentryposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    journalentryid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isbanktransactionposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    banktransactionid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isjournalentrytransferfeeposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    journalentrytransferfeeid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isbanktransactiontransferfeeposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    banktransactiontransferfeeid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isjournalentryvoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    journalentryvoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isbanktransactionvoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    banktransactionvoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isjournalentrytransferfeevoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    journalentrytransferfeevoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isbanktransactiontransferfeevoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    banktransactiontransferfeevoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("pk_quotationsdetails", x => x.id);
                    table.ForeignKey(
                        name: "fk_quotationsdetails_banks_banksourceid",
                        column: x => x.banksourceid,
                        principalSchema: "bco",
                        principalTable: "banks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotationsdetails_banks_banktargetid",
                        column: x => x.banktargetid,
                        principalSchema: "bco",
                        principalTable: "banks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotationsdetails_currencies_currencydetailid",
                        column: x => x.currencydetailid,
                        principalSchema: "cnf",
                        principalTable: "currencies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_quotationsdetails_quotations_parentid",
                        column: x => x.parentid,
                        principalSchema: "fac",
                        principalTable: "quotations",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                schema: "bco",
                table: "banks",
                columns: new[] { "id", "bankaccountexcludeuid", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "logolocalpath", "logourl", "name", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, new Guid("9f8a706a-f0c4-4bb0-9159-d9f0af666152"), "BAC", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/BacLogo.png", "Banco de America Central", null, null, null, null },
                    { 2, null, "BDF", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/BdfLogo.png", "Banco de Finanza", null, null, null, null },
                    { 3, null, "LAFISE", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/LafiseLogo.png", "Bancentro", null, null, null, null },
                    { 4, null, "ATLANT", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/AtlantidaLogo.png", "ATLANTIDA", null, null, null, null },
                    { 5, new Guid("bbcf016d-4cdd-43b7-99b0-bea2375ce2ce"), "FICOHSA", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/FicohsaLogo.png", "FICOHSA", null, null, null, null },
                    { 6, null, "BANPRO", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/BanproLogo.png", "BANPRO", null, null, null, null },
                    { 7, null, "AVANZ", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, null, "/Content/images/Bank/AvanzLogo.png", "AVANZ", null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "companies",
                columns: new[] { "id", "addressprimary", "billingauthorizationnumber", "businessname", "commercialname", "countrycode", "countrynumber", "createdby", "createddate", "createdhostname", "createdipv4", "identificationnumber", "imagelogolocalpath", "imagelogourl", "imagesplashlocalpath", "imagesplashurl", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "journalauthorizationnumber", "name", "phonenumber", "updatedby", "updateddate", "updatedhostname", "updatedipv4", "usebranch", "website" },
                values: new object[] { 1, "Portón Principal del Colegio Teresiano 1/2c.al este. Managua, Nicaragua", null, "Factoring S.A.", "Factoring S.A.", "NIC", (short)558, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "J0310000031339", "wwwroot/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg", "https://localhost:7102/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg", null, null, null, null, null, null, true, null, "Factoring S.A.", "+505 22782272", null, null, null, null, true, "https://factoring.com.ni" });

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "configscxc",
                columns: new[] { "id", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "isautomaticallycustomercode", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[] { 1, 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", true, null, null, null, null });

            migrationBuilder.InsertData(
                schema: "fac",
                table: "configsfac",
                columns: new[] { "id", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "isautomaticallyquotationcode", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[] { 1, 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", true, null, null, null, null });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "currencies",
                columns: new[] { "id", "abbreviation", "code", "codeiso", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "name", "nameforeign", "nameforeignsingular", "namesingular", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "C$", "COR", "NIO", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "CORDOBAS", "CORDOBAS", "CORDOBA", "CORDOBA", 1, null, null, null, null },
                    { 2, "U$", "USD", "USD", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "DOLARES", "DOLLARS", "DOLLAR", "DOLAR", 2, null, null, null, null },
                    { 4, "€", "EUR", "EUR", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "EUROS", "EUROS", "EURO", "EURO", 4, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "isbank", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "BAN", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, true, "Bancos", 1, null, null, null, null },
                    { 2, "FIN", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, true, "Financieras", 2, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "customerscategories",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 3, "IND", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Industrias", 3, null, null, null, null },
                    { 4, "ONG", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "ONG", 4, null, null, null, null },
                    { 5, "UNI", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Universidades", 5, null, null, null, null },
                    { 6, "COM", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Comercial", 6, null, null, null, null },
                    { 7, "FAM", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Farmacias", 7, null, null, null, null },
                    { 8, "TEC", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Tecnológicos", 8, null, null, null, null },
                    { 9, "OTR", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Otros", 9, null, null, null, null },
                    { 10, "SER", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", null, null, null, null, true, "Servicios", 10, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "identificationstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "formatexpressionnumber", "identificationmaxlength", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "isforeign", "islegal", "name", "numeral", "regularexpressionnumber", "substitutionexpressionnumber", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "RUC", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "$1", (short)14, null, null, null, null, true, false, true, "Registro Único Cotnribuyente", 1, "([J0-9]\\\\d{12}[a-zA-Z0-9])", "$1", null, null, null, null },
                    { 2, "CEDU", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "$1-$2-$3", (short)14, null, null, null, null, true, false, false, "Cédula de Identificación", 2, "(\\d{3})-*?(\\d{6})-*?(\\d{4}\\w{1})", "$1$2$3", null, null, null, null },
                    { 3, "DIMEX", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "", (short)0, null, null, null, null, false, true, false, "Documento de Identidad Migratorio para Extranjeros", 4, "", "", null, null, null, null },
                    { 4, "NITE", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "", (short)0, null, null, null, null, false, false, false, "Número de Identificación Tributaria Especial", 8, "", "", null, null, null, null },
                    { 5, "DIDI", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "", (short)0, null, null, null, null, false, true, false, "Documento de Identificación para Diplomático", 16, "", "", null, null, null, null },
                    { 6, "PASS", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "", (short)0, null, null, null, null, true, true, false, "Pasaporte", 32, "", "", null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "personstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "name", "numeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "NAT", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "Natural", 1, null, null, null, null },
                    { 2, "JUR", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "Jurídico", 2, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "fac",
                table: "quotationstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "name", "numeral", "ordersequence", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "COM", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "COMPRA", 1, (short)10, null, null, null, null },
                    { 2, "VTA", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "VENTA", 2, (short)20, null, null, null, null },
                    { 3, "TRF", 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", "TRANSFERENCIA", 4, (short)30, null, null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_banks_companyid_code",
                schema: "bco",
                table: "banks",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_banksaccounts_companyid_parentid_code",
                schema: "bco",
                table: "banksaccounts",
                columns: new[] { "companyid", "parentid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_banksaccounts_currencyid",
                schema: "bco",
                table: "banksaccounts",
                column: "currencyid");

            migrationBuilder.CreateIndex(
                name: "ix_banksaccounts_parentid",
                schema: "bco",
                table: "banksaccounts",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_businessexecutives_companyid_code",
                schema: "cxc",
                table: "businessexecutives",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_companies_countrycode_identificationnumber",
                schema: "cnf",
                table: "companies",
                columns: new[] { "countrycode", "identificationnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_companies_countrynumber_identificationnumber",
                schema: "cnf",
                table: "companies",
                columns: new[] { "countrynumber", "identificationnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_configscxc_companyid",
                schema: "cxc",
                table: "configscxc",
                column: "companyid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_configsfac_companyid",
                schema: "fac",
                table: "configsfac",
                column: "companyid",
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
                name: "ix_currenciesexchangerates_companyid_currencyid_datetransa",
                schema: "cnf",
                table: "currenciesexchangerates",
                columns: new[] { "companyid", "currencyid", "datetransa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currenciesexchangerates_companyid_currencytype_datetransa",
                schema: "cnf",
                table: "currenciesexchangerates",
                columns: new[] { "companyid", "currencytype", "datetransa" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currenciesexchangerates_currencyid",
                schema: "cnf",
                table: "currenciesexchangerates",
                column: "currencyid");

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_internalserial_code",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "internalserial", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_typeid_identificationnumber",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "typeid", "identificationnumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_identificationtypeid",
                schema: "cxc",
                table: "customers",
                column: "identificationtypeid");

            migrationBuilder.CreateIndex(
                name: "ix_customers_sectorid",
                schema: "cxc",
                table: "customers",
                column: "sectorid");

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
                name: "ix_customerssectors_companyid_code",
                schema: "cxc",
                table: "customerssectors",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customerssectors_parentid",
                schema: "cxc",
                table: "customerssectors",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_identificationstypes_companyid_code",
                schema: "cnf",
                table: "identificationstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identificationstypes_companyid_numeral",
                schema: "cnf",
                table: "identificationstypes",
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
                name: "ix_quotations_bankaccountsourceid",
                schema: "fac",
                table: "quotations",
                column: "bankaccountsourceid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_bankaccounttargetid",
                schema: "fac",
                table: "quotations",
                column: "bankaccounttargetid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_businessexecutiveid",
                schema: "fac",
                table: "quotations",
                column: "businessexecutiveid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_companyid_typeid_datetransa_internalserial_numeral",
                schema: "fac",
                table: "quotations",
                columns: new[] { "companyid", "typeid", "datetransa", "internalserial", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencydepositid",
                schema: "fac",
                table: "quotations",
                column: "currencydepositid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencytransaid",
                schema: "fac",
                table: "quotations",
                column: "currencytransaid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencytransferid",
                schema: "fac",
                table: "quotations",
                column: "currencytransferid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_customerid",
                schema: "fac",
                table: "quotations",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_typeid",
                schema: "fac",
                table: "quotations",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "ix_quotationsdetails_banksourceid",
                schema: "fac",
                table: "quotationsdetails",
                column: "banksourceid");

            migrationBuilder.CreateIndex(
                name: "ix_quotationsdetails_banktargetid",
                schema: "fac",
                table: "quotationsdetails",
                column: "banktargetid");

            migrationBuilder.CreateIndex(
                name: "ix_quotationsdetails_companyid_parentid_quotationdetailtype_linenumber",
                schema: "fac",
                table: "quotationsdetails",
                columns: new[] { "companyid", "parentid", "quotationdetailtype", "linenumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotationsdetails_currencydetailid",
                schema: "fac",
                table: "quotationsdetails",
                column: "currencydetailid");

            migrationBuilder.CreateIndex(
                name: "ix_quotationsdetails_parentid",
                schema: "fac",
                table: "quotationsdetails",
                column: "parentid");

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
                name: "companies",
                schema: "cnf");

            migrationBuilder.DropTable(
                name: "configscxc",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "configsfac",
                schema: "fac");

            migrationBuilder.DropTable(
                name: "currenciesexchangerates",
                schema: "cnf");

            migrationBuilder.DropTable(
                name: "customerscategories",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "quotationsdetails",
                schema: "fac");

            migrationBuilder.DropTable(
                name: "quotations",
                schema: "fac");

            migrationBuilder.DropTable(
                name: "banksaccounts",
                schema: "bco");

            migrationBuilder.DropTable(
                name: "businessexecutives",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "quotationstypes",
                schema: "fac");

            migrationBuilder.DropTable(
                name: "banks",
                schema: "bco");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "cnf");

            migrationBuilder.DropTable(
                name: "customerssectors",
                schema: "cxc");

            migrationBuilder.DropTable(
                name: "identificationstypes",
                schema: "cnf");

            migrationBuilder.DropTable(
                name: "personstypes",
                schema: "cnf");
        }
    }
}
