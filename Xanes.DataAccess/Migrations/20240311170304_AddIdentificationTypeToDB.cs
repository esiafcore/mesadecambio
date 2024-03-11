using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentificationTypeToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "identificationstypes",
                schema: "cnf",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    persontypeid = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    islegal = table.Column<bool>(type: "bit", nullable: false),
                    isforeign = table.Column<bool>(type: "bit", nullable: false),
                    regularexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    formatexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    substitutionexpressionnumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
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

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2780));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2782));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2785));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2776));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2787));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(2789));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5274));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5280));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5283));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5284));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5286));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5288));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5289));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5344));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5346));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(5348));

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "identificationstypes",
                columns: new[] { "id", "code", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "formatexpressionnumber", "identificationmaxlength", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "isforeign", "islegal", "name", "persontypeid", "regularexpressionnumber", "substitutionexpressionnumber", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 1, "RUC", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7321), "", "", "$1", (short)14, null, null, null, null, true, false, true, "Registro Único Cotnribuyente", 1, "([J0-9]\\\\d{12}[a-zA-Z0-9])", "$1", null, null, null, null },
                    { 2, "CEDU", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7331), "", "", "$1-$2-$3", (short)14, null, null, null, null, true, false, false, "Cédula de Identificación", 2, "(\\d{3})-*?(\\d{6})-*?(\\d{4}\\w{1})", "$1$2$3", null, null, null, null },
                    { 3, "DIMEX", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7334), "", "", "", (short)0, null, null, null, null, false, true, false, "Documento de Identidad Migratorio para Extranjeros", 4, "", "", null, null, null, null },
                    { 4, "NITE", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7336), "", "", "", (short)0, null, null, null, null, false, false, false, "Número de Identificación Tributaria Especial", 8, "", "", null, null, null, null },
                    { 5, "DIDI", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7339), "", "", "", (short)0, null, null, null, null, false, true, false, "Documento de Identificación para Diplomático", 16, "", "", null, null, null, null },
                    { 6, "PASS", 1, "", new DateTime(2024, 3, 11, 17, 3, 3, 9, DateTimeKind.Utc).AddTicks(7341), "", "", "", (short)0, null, null, null, null, true, true, false, "Pasaporte", 32, "", "", null, null, null, null }
                });

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(2519));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 8, DateTimeKind.Utc).AddTicks(2524));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 3, 3, 7, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.CreateIndex(
                name: "ix_identificationstypes_companyid_code",
                schema: "cnf",
                table: "identificationstypes",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identificationstypes_companyid_persontypeid",
                schema: "cnf",
                table: "identificationstypes",
                columns: new[] { "companyid", "persontypeid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "identificationstypes",
                schema: "cnf");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7271));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7297));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 226, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1431));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1437));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(1440));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8288));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8294));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8296));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8297));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8299));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8300));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8302));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8303));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8305));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(8306));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(5894));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(5902));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 4, 5, 19, 227, DateTimeKind.Utc).AddTicks(3582));
        }
    }
}
