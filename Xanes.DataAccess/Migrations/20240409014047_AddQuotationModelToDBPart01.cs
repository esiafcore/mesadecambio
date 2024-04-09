using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationModelToDBPart01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quotations",
                schema: "cxc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    datetransa = table.Column<DateOnly>(type: "date", nullable: false),
                    typeid = table.Column<int>(type: "int", nullable: false),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    customerid = table.Column<int>(type: "int", nullable: false),
                    currencyoriginexchangeid = table.Column<int>(type: "int", nullable: false),
                    currencytransaid = table.Column<int>(type: "int", nullable: false),
                    exchangerateofficialtransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratebuytransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangerateselltransa = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangerateofficialreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratebuyreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    exchangeratesellreal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    amounttransa = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    amountrevenue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    amountcost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
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
                        name: "fk_quotations_currencies_currencyoriginexchangeid",
                        column: x => x.currencyoriginexchangeid,
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

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2022));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2042));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2044));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2046));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2038));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2052));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(2054));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(6305));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6630));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6638));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(6641));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6863));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6869));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6873));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6875));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6877));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6878));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6880));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6882));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(6884));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4220));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4227));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4231));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4233));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4236));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 337, DateTimeKind.Utc).AddTicks(4238));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(1881));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 336, DateTimeKind.Utc).AddTicks(1886));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9157));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 4, 9, 1, 40, 46, 335, DateTimeKind.Utc).AddTicks(9172));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_companyid_typeid_datetransa_numeral",
                schema: "cxc",
                table: "quotations",
                columns: new[] { "companyid", "typeid", "datetransa", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencyoriginexchangeid",
                schema: "cxc",
                table: "quotations",
                column: "currencyoriginexchangeid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_currencytransaid",
                schema: "cxc",
                table: "quotations",
                column: "currencytransaid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_customerid",
                schema: "cxc",
                table: "quotations",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "ix_quotations_typeid",
                schema: "cxc",
                table: "quotations",
                column: "typeid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quotations",
                schema: "cxc");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2238));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2271));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2274));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2277));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2263));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 831, DateTimeKind.Utc).AddTicks(2283));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 834, DateTimeKind.Utc).AddTicks(1415));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(61));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(72));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(920));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(926));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(929));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(931));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(933));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(935));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(938));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(940));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(942));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(944));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(8979));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(8988));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9061));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9064));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9067));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 833, DateTimeKind.Utc).AddTicks(9070));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(5880));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(5886));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3033));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 19, 3, 3, 37, 832, DateTimeKind.Utc).AddTicks(3043));
        }
    }
}
