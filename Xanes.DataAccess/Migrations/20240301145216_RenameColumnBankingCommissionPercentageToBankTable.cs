using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xanes.DataAccess.Migrations;

/// <inheritdoc />
public partial class RenameColumnBankingCommissionPercentageToBankTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "comisionbancariaporcentaje",
            schema: "bco",
            table: "banks",
            newName: "bankingcommissionpercentage");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "bankingcommissionpercentage",
            schema: "bco",
            table: "banks",
            newName: "comisionbancariaporcentaje");
    }
}
