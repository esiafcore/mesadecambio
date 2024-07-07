using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations.PgSql
{
    /// <inheritdoc />
    public partial class AddColumnsTransferFeeToQuotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "banktransactiontransferfeeid",
                schema: "fac",
                table: "quotationsdetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "banktransactiontransferfeevoidid",
                schema: "fac",
                table: "quotationsdetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isbanktransactiontransferfeeposted",
                schema: "fac",
                table: "quotationsdetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isbanktransactiontransferfeevoidposted",
                schema: "fac",
                table: "quotationsdetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isjournalentrytransferfeeposted",
                schema: "fac",
                table: "quotationsdetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isjournalentrytransferfeevoidposted",
                schema: "fac",
                table: "quotationsdetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "journalentrytransferfeeid",
                schema: "fac",
                table: "quotationsdetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "journalentrytransferfeevoidid",
                schema: "fac",
                table: "quotationsdetails",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banktransactiontransferfeeid",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "banktransactiontransferfeevoidid",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "isbanktransactiontransferfeeposted",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "isbanktransactiontransferfeevoidposted",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "isjournalentrytransferfeeposted",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "isjournalentrytransferfeevoidposted",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "journalentrytransferfeeid",
                schema: "fac",
                table: "quotationsdetails");

            migrationBuilder.DropColumn(
                name: "journalentrytransferfeevoidid",
                schema: "fac",
                table: "quotationsdetails");
        }
    }
}
