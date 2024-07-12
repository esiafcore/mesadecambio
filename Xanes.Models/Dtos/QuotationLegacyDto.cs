using System.ComponentModel.DataAnnotations;

namespace Xanes.Models.Dtos;

public class QuotationLegacyDto
{
    public int Id { get; set; }

    public string TypeCode { get; set; } = string.Empty;

    public int NumeralTrx { get; set; } = 0;

    public string DateTrx { get; set; } = string.Empty;

    public string CustomerCode { get; set; } = string.Empty;

    public string CustomerIdentificationNumber { get; set; } = string.Empty;

    public string BusinessName { get; set; } = string.Empty;

    public int CurrencyTransaction { get; set; } = 0;

    public int CurrencyDeposit { get; set; } = 0;

    public int CurrencyTransfer { get; set; } = 0;

    public decimal ExchangeRateOfficial { get; set; } = 0M;

    public decimal ExchangeRateBuy { get; set; } = 0M;

    public decimal ExchangeRateSell { get; set; } = 0M;

    public decimal AmountTrx { get; set; } = 0M;

    public decimal AmountExchange { get; set; } = 0M;

    public decimal AmountCost { get; set; } = 0M;

    public decimal AmountRevenue { get; set; } = 0M;

    public decimal AmountTransferFee { get; set; } = 0M;

    public int? BankAccountSourceId { get; set; } = null;

    public string? BankAccountSourceCode { get; set; } = null;

    public string? BankSourceCode { get; set; } = null;

    public int? BankAccountTargetId { get; set; } = null;

    public string? BankAccountTargetCode { get; set; } = null;

    public string? BankTargetCode { get; set; } = null;

    public string BusinessExecutiveCode { get; set; } = string.Empty;

    public bool IsClosed { get; set; }

    public bool IsJournalPost { get; set; }

    public bool IsVoid { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime CreatedOn { get; set; } = new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015);

    public string CreatedBy { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? UpdatedOn { get; set; } = null;

    public string? UpdatedBy { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ClosedOn { get; set; } = null;

    public string? ClosedBy { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? ReClosedOn { get; set; } = null;

    public string? ReClosedBy { get; set; } = null;
}