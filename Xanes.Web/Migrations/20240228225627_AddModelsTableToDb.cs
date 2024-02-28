using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddModelsTableToDb : Migration
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
                    companyid = table.Column<int>(type: "int", nullable: false)
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
                    companyid = table.Column<int>(type: "int", nullable: false)
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
                    companyid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotationstypes", x => x.id);
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
