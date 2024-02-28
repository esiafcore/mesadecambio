using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationTypeTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numeral = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    OrderSequence = table.Column<short>(type: "smallint", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationsTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "currencies_idx_2040",
                table: "Currencies",
                columns: new[] { "CompanyId", "Numeral" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "quotationstypes_idx_2010",
                table: "QuotationsTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "quotationstypes_idx_2020",
                table: "QuotationsTypes",
                columns: new[] { "CompanyId", "Numeral" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationsTypes");

            migrationBuilder.DropIndex(
                name: "currencies_idx_2040",
                table: "Currencies");
        }
    }
}
