using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_BusinessExecutives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "businessexecutivecode",
                schema: "fac",
                table: "quotations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "businessexecutiveid",
                schema: "fac",
                table: "quotations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 10, 3, 44, 40, 272, DateTimeKind.Utc).AddTicks(3225));

            migrationBuilder.CreateIndex(
                name: "ix_quotations_businessexecutiveid",
                schema: "fac",
                table: "quotations",
                column: "businessexecutiveid");

            migrationBuilder.CreateIndex(
                name: "ix_businessexecutives_companyid_code",
                schema: "cxc",
                table: "businessexecutives",
                columns: new[] { "companyid", "code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_quotations_businessexecutives_businessexecutiveid",
                schema: "fac",
                table: "quotations",
                column: "businessexecutiveid",
                principalSchema: "cxc",
                principalTable: "businessexecutives",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_quotations_businessexecutives_businessexecutiveid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropTable(
                name: "businessexecutives",
                schema: "cxc");

            migrationBuilder.DropIndex(
                name: "ix_quotations_businessexecutiveid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "businessexecutivecode",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "businessexecutiveid",
                schema: "fac",
                table: "quotations");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 9, 3, 50, 38, 394, DateTimeKind.Utc).AddTicks(4675));
        }
    }
}
