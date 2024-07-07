using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_Customer_Remove_Category_P001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropIndex(
                name: "ix_customers_categoryid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "categoryid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "categorynumeral",
                schema: "cxc",
                table: "customers");

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 9, 3, 50, 38, 394, DateTimeKind.Utc).AddTicks(4675));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categoryid",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "categorynumeral",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "cnf",
                table: "companies",
                keyColumn: "id",
                keyValue: 1,
                column: "createddate",
                value: new DateTime(2024, 5, 5, 7, 6, 5, 392, DateTimeKind.Utc).AddTicks(7383));

            migrationBuilder.CreateIndex(
                name: "ix_customers_categoryid",
                schema: "cxc",
                table: "customers",
                column: "categoryid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_customerscategories_categoryid",
                schema: "cxc",
                table: "customers",
                column: "categoryid",
                principalSchema: "cxc",
                principalTable: "customerscategories",
                principalColumn: "id");
        }
    }
}
