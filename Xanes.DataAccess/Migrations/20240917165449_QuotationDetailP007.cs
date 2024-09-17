using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class QuotationDetailP007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "amountdetail",
                schema: "fac",
                table: "quotationsdetails",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.AddColumn<decimal>(
                name: "amountcost",
                schema: "fac",
                table: "quotationsdetails",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "amountrevenue",
                schema: "fac",
                table: "quotationsdetails",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "percentagecostrevenue",
                schema: "fac",
                table: "quotationsdetails",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountcost",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "amountrevenue",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "percentagecostrevenue",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "amountdetail",
                schema: "fac",
                table: "quotationsdetails",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldDefaultValue: 0m);
        }
    }
}
