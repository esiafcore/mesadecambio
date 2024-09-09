using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldQuotationP002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isapproved",
                schema: "fac",
                table: "quotations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isapproved",
                schema: "fac",
                table: "quotations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
