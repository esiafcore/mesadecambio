using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_QuotationModelP005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isvoid",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "isapproved",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isapproved",
                schema: "fac",
                table: "quotations");

            migrationBuilder.AlterColumn<bool>(
                name: "isvoid",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
