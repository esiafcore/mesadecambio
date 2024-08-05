using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FIX_Customer_P001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "businessexecutiveid",
                schema: "cxc",
                table: "customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_businessexecutiveid",
                schema: "cxc",
                table: "customers",
                column: "businessexecutiveid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_businessexecutives_businessexecutiveid",
                schema: "cxc",
                table: "customers",
                column: "businessexecutiveid",
                principalSchema: "cxc",
                principalTable: "businessexecutives",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_businessexecutives_businessexecutiveid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropIndex(
                name: "ix_customers_businessexecutiveid",
                schema: "cxc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "businessexecutiveid",
                schema: "cxc",
                table: "customers");
        }
    }
}
