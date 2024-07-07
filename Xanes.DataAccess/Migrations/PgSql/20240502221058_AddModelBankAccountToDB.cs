using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class AddModelBankAccountToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 2, 22, 10, 56, 724, DateTimeKind.Utc).AddTicks(6647));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banksaccounts",
                schema: "bco");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 1, 4, 4, 1, 181, DateTimeKind.Utc).AddTicks(6425));
        }
    }
}
