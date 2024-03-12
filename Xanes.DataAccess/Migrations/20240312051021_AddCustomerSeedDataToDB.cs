using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerSeedDataToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customers_companyid_identificationnumber",
                schema: "cxc",
                table: "customers");

            migrationBuilder.AddColumn<int>(
                name: "categorynumeral",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "typenumeral",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2164));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2167));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2169));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2171));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(2173));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5867));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5875));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(5878));

            migrationBuilder.InsertData(
                schema: "cxc",
                table: "customers",
                columns: new[] { "id", "businessname", "categoryid", "categorynumeral", "code", "commercialname", "companyid", "createdby", "createddate", "createdhostname", "createdipv4", "firstname", "identificationnumber", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "isbank", "issystemrow", "lastname", "secondname", "secondsurname", "typeid", "typenumeral", "updatedby", "updateddate", "updatedhostname", "updatedipv4" },
                values: new object[,]
                {
                    { 5805, "MEYLING RAQUEL SANCHEZ ORTIZ", 6, 6, "00799", "MEYLING RAQUEL SANCHEZ ORTIZ", 1, "", new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8815), "", "", "MEYLING", "0012206860039E", null, null, null, null, true, false, false, "SANCHEZ", "RAQUEL", "ORTIZ", 2, 2, null, null, null, null },
                    { 5806, "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA", 6, 6, "00800", "INSUMOS SMART NICARAGUA SOCIEDAD ANONIMA", 1, "", new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8812), "", "", "", "J0310000441430", null, null, null, null, true, false, false, "", "", "", 1, 1, null, null, null, null },
                    { 5807, "JIMMY ALEXANDER SANDOVAL FRANCO", 6, 6, "00801", "JIMMY ALEXANDER SANDOVAL FRANCO", 1, "", new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8809), "", "", "JIMMY", "244686858", null, null, null, null, true, false, false, "SANDOVAL", "ALEXANDER", "FRANCO", 2, 2, null, null, null, null },
                    { 5808, "MIGUEL FERNANDO RAMIREZ OCON", 6, 6, "00802", "MIGUEL FERNANDO RAMIREZ OCON", 1, "", new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8805), "", "", "MIGUEL", "0013009870051Y", null, null, null, null, true, false, false, "RAMIREZ", "FERNANDO", "OCON", 2, 2, null, null, null, null },
                    { 5809, "AMERICAN PHARMA", 6, 6, "00803", "AMERICAN PHARMA", 1, "", new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(8789), "", "", "", "J0310000122865", null, null, null, null, true, false, false, "", "", "", 1, 1, null, null, null, null }
                });

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2561));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2566));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2568));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2570));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2572));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2573));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2575));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2576));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2578));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(2580));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1292));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1298));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1304));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1306));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 480, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(239));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 479, DateTimeKind.Utc).AddTicks(243));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7946));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7952));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 12, 5, 10, 20, 478, DateTimeKind.Utc).AddTicks(7953));

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_typeid_identificationnumber",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "typeid", "identificationnumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customers_companyid_typeid_identificationnumber",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DeleteData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5805);

            migrationBuilder.DeleteData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5806);

            migrationBuilder.DeleteData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5807);

            migrationBuilder.DeleteData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5808);

            migrationBuilder.DeleteData(
                schema: "cxc",
                table: "customers",
                keyColumn: "id",
                keyValue: 5809);

            migrationBuilder.DropColumn(
                name: "categorynumeral",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "typenumeral",
                schema: "cxc",
                table: "customers");

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8630));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8651));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8653));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8655));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8647));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8657));

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 209, DateTimeKind.Utc).AddTicks(8659));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2540));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2547));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(2549));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9234));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9236));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9238));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9239));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9241));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9242));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9244));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9245));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(9247));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7966));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7973));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7976));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7978));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7981));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 211, DateTimeKind.Utc).AddTicks(7983));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(6964));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(6968));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4649));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4655));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 11, 17, 54, 47, 210, DateTimeKind.Utc).AddTicks(4657));

            migrationBuilder.CreateIndex(
                name: "ix_customers_companyid_identificationnumber",
                schema: "cxc",
                table: "customers",
                columns: new[] { "companyid", "identificationnumber" },
                unique: true);
        }
    }
}
