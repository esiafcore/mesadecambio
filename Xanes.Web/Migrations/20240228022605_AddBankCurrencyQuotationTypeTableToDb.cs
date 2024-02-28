using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBankCurrencyQuotationTypeTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    comisionbancariaporcentaje = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    bankaccountexcludeuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    iscompany = table.Column<bool>(type: "bit", nullable: false),
                    orderpriority = table.Column<int>(type: "int", nullable: false),
                    logobank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_banks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
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
                    companyid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quotationstypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeral = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ordersequence = table.Column<short>(type: "smallint", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotationstypes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_banks_companyid_code",
                table: "banks",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_abbreviation",
                table: "currencies",
                columns: new[] { "companyid", "abbreviation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_code",
                table: "currencies",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_codeiso",
                table: "currencies",
                columns: new[] { "companyid", "codeiso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_currencies_companyid_numeral",
                table: "currencies",
                columns: new[] { "companyid", "numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotationstypes_companyid_code",
                table: "quotationstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_quotationstypes_companyid_numeral",
                table: "quotationstypes",
                columns: new[] { "companyid", "numeral" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "currencies");

            migrationBuilder.DropTable(
                name: "quotationstypes");
        }
    }
}
