using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInternalSerialToQuotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_quotations_companyid_typeid_datetransa_numeral",
                schema: "fac",
                table: "quotations");

            migrationBuilder.AddColumn<string>(
                name: "internalserial",
                schema: "fac",
                table: "quotations",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "Z");

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

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 23, 17, 34, 4, 494, DateTimeKind.Utc).AddTicks(2261));

            migrationBuilder.InsertData(
                schema: "fac",
                table: "configsfac",
                columns: new[] { "id", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "isautomaticallyquotationcode", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[] { 1, 1, "", new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015), "", "", true, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "ix_quotations_companyid_typeid_datetransa_internalserial_numeral",
                schema: "fac",
                table: "quotations",
                columns: new[] { "companyid", "typeid", "datetransa", "internalserial", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_configsfac_companyid",
                schema: "fac",
                table: "configsfac",
                column: "companyid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configsfac",
                schema: "fac");

            migrationBuilder.DropIndex(
                name: "ix_quotations_companyid_typeid_datetransa_internalserial_numeral",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "internalserial",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 4, 23, 15, 56, 2, 878, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_companyid_typeid_datetransa_numeral",
                schema: "fac",
                table: "quotations",
                columns: new[] { "companyid", "typeid", "datetransa", "numeral" },
                unique: true);
        }
    }
}
