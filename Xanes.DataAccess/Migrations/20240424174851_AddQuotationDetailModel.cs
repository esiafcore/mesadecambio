using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isvoid",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                    isjournalentryvoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    journalentryvoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isbanktransactionvoidposted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    banktransactionvoidid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 24, 17, 48, 50, 48, DateTimeKind.Utc).AddTicks(8886));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quotationsdetails",
                schema: "fac");

            migrationBuilder.DropColumn(
                name: "isvoid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 23, 17, 34, 4, 494, DateTimeKind.Utc).AddTicks(2261));
        }
    }
}
