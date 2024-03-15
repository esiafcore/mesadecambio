using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyModelToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logobank",
                schema: "bco",
                table: "banks");

            migrationBuilder.AddColumn<string>(
                name: "logolocalpath",
                schema: "bco",
                table: "banks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "logourl",
                schema: "bco",
                table: "banks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

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

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1739), null, "/Content/images/Bank/BacLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1761), null, "/Content/images/Bank/BdfLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1763), null, "/Content/images/Bank/LafiseLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1764), null, "/Content/images/Bank/AtlantidaLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1758), null, "/Content/images/Bank/FicohsaLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1766), null, "/Content/images/Bank/BanproLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "createddate", "logolocalpath", "logourl" },
                values: new object[] { new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(1768), null, "/Content/images/Bank/AvanzLogo.png" });

            migrationBuilder.InsertData(
                schema: "cnf",
                table: "companies",
                columns: new[] { "id", "addressprimary", "billingauthorizationnumber", "businessname", "commercialname", "countrycode", "countrynumber", "createdby", "createddate", "createdhostname", "createdipv4", "identificationnumber", "imagelogolocalpath", "imagelogourl", "imagesplashlocalpath", "imagesplashurl", "inactivatedby", "inactivateddate", "inactivatedhostname", "inactivatedipv4", "isactive", "journalauthorizationnumber", "name", "phonenumber", "updatedby", "updateddate", "updatedhostname", "updatedipv4", "usebranch", "website" },
                values: new object[] { 1, "Portón Principal del Colegio Teresiano 1/2c.al este. Managua, Nicaragua", null, "Factoring S.A.", "Factoring S.A.", "NIC", (short)558, "", new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(4565), "", "", "J0310000031339", "wwwroot/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg", "https://localhost:7102/CompanyImagesLogo/8071a404-950c-4df1-9875-27db4f4a4c26.jpg", null, null, null, null, null, null, true, null, "Factoring S.A.", "+505 22782272", null, null, null, null, true, "https://factoring.com.ni" });

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(5767));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(5776));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(5779));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7494));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7503));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7504));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7505));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7506));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7508));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7509));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7510));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(7512));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3098));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3104));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3106));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3108));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3115));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 5, DateTimeKind.Utc).AddTicks(3116));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(3394));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 4, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 15, 0, 59, 38, 3, DateTimeKind.Utc).AddTicks(9872));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companies",
                schema: "cnf");

            migrationBuilder.DropColumn(
                name: "logolocalpath",
                schema: "bco",
                table: "banks");

            migrationBuilder.DropColumn(
                name: "logourl",
                schema: "bco",
                table: "banks");

            migrationBuilder.AddColumn<string>(
                name: "logobank",
                schema: "bco",
                table: "banks",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3107), "/Content/images/Bank/BacLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3127), "/Content/images/Bank/BdfLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3128), "/Content/images/Bank/LafiseLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3133), "/Content/images/Bank/AtlantidaLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3122), "/Content/images/Bank/FicohsaLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3135), "/Content/images/Bank/BanproLogo.png" });

            migrationBuilder.UpdateData(
                schema: "bco",
                table: "banks",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "createddate", "logobank" },
                values: new object[] { new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(3161), "/Content/images/Bank/AvanzLogo.png" });

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(6508));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(6518));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "currencies",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(6520));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2301));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2306));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2308));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2309));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2311));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2312));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 7,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2313));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 8,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2314));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 9,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2316));

            migrationBuilder.UpdateData(
                schema: "cxc",
                table: "customerscategories",
                keyColumn: "id",
                keyValue: 10,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(2317));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7510));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7517));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7520));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 4,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7522));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 5,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7523));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "identificationstypes",
                keyColumn: "id",
                keyValue: 6,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(7525));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "personstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 896, DateTimeKind.Utc).AddTicks(304));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(8301));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 2,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(8307));

            migrationBuilder.UpdateData(
                schema: "fac",
                table: "quotationstypes",
                keyColumn: "id",
                keyValue: 3,
                column: "createddate",
                value: new DateTime(2024, 3, 14, 21, 33, 55, 895, DateTimeKind.Utc).AddTicks(8309));
        }
    }
}
