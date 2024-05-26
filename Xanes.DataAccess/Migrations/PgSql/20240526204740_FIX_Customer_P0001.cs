using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_Customer_P0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identificationtypecode",
                schema: "cxc",
                table: "customers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "identificationtypeid",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "identificationtypenumber",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_customers_identificationtypeid",
                schema: "cxc",
                table: "customers",
                column: "identificationtypeid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_identificationstypes_identificationtypeid",
                schema: "cxc",
                table: "customers",
                column: "identificationtypeid",
                principalSchema: "cnf",
                principalTable: "identificationstypes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_identificationstypes_identificationtypeid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropIndex(
                name: "ix_customers_identificationtypeid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "identificationtypecode",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "identificationtypeid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "identificationtypenumber",
                schema: "cxc",
                table: "customers");
        }
    }
}
