using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FIX_Quotation_AddFieldReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "amountcostreal",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "amountrevenuereal",
                schema: "fac",
                table: "quotations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amountcostreal",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "amountrevenuereal",
                schema: "fac",
                table: "quotations");
        }
    }
}
