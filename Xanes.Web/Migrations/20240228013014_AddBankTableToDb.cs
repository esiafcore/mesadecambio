using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddBankTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ComisionBancariaPorcentaje = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BankAccountExcludeUId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsCompany = table.Column<bool>(type: "bit", nullable: false),
                    OrderPriority = table.Column<int>(type: "int", nullable: false),
                    LogoBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
