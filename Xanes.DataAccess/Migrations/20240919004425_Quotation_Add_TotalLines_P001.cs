using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Quotation_Add_TotalLines_P001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "totaldepositlines",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "totallines",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "totaltransferlines",
                schema: "fac",
                table: "quotations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totaldepositlines",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "totallines",
                schema: "fac",
                table: "quotations");

            migrationBuilder.DropColumn(
                name: "totaltransferlines",
                schema: "fac",
                table: "quotations");
        }
    }
}
