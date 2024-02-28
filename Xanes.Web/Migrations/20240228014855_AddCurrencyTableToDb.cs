using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeIso = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameSingular = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameFor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameForSingular = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Numeral = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "banks_idx_2010",
                table: "Banks",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "currencies_idx_2010",
                table: "Currencies",
                columns: new[] { "CompanyId", "CodeIso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "currencies_idx_2020",
                table: "Currencies",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "currencies_idx_2030",
                table: "Currencies",
                columns: new[] { "CompanyId", "Abbreviation" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "banks_idx_2010",
                table: "Banks");
        }
    }
}
