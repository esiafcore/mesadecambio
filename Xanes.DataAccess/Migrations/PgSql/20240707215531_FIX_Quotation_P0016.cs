using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class FIX_Quotation_P0016 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isadjustment",
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
                name: "isadjustment",
                schema: "fac",
                table: "quotations");
        }
    }
}
