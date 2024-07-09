using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class QuotationLegacyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("typeCode")]
    public string TypeCode { get; set; } = string.Empty;

    [JsonPropertyName("numeralTrx")]
    public int NumeralTrx { get; set; } = 0;

    [JsonPropertyName("dateTrx")]
    public string DateTrx { get; set; } = string.Empty;

    [JsonPropertyName("customerCode")]
    public string CustomerCode { get; set; } = string.Empty;

    [JsonPropertyName("businessName")]
    public string BusinessName { get; set; } = string.Empty;

    [JsonPropertyName("currencyTransaction")]
    public int CurrencyTransaction { get; set; } = 0;

    [JsonPropertyName("currencyDeposit")]
    public int CurrencyDeposit { get; set; } = 0;

    [JsonPropertyName("currencyTransfer")]
    public int CurrencyTransfer { get; set; } = 0;

    [JsonPropertyName("exchangeRateOfficial")]
    public decimal ExchangeRateOfficial { get; set; } = 0M;

    [JsonPropertyName("exchangeRateBuy")]
    public decimal ExchangeRateBuy { get; set; } = 0M;

    [JsonPropertyName("exchangeRateSell")]
    public decimal ExchangeRateSell { get; set; } = 0M;

    [JsonPropertyName("amountTrx")]
    public decimal AmountTrx { get; set; } = 0M;

    [JsonPropertyName("amountExchange")]
    public decimal AmountExchange { get; set; } = 0M;

    [JsonPropertyName("amountCost")]
    public decimal AmountCost { get; set; } = 0M;

    [JsonPropertyName("amountRevenue")]
    public decimal AmountRevenue { get; set; } = 0M;

    [JsonPropertyName("amountTransferFee")]
    public decimal AmountTransferFee { get; set; } = 0M;

    [JsonPropertyName("bankAccountSourceId")]
    public int BankAccountSourceId { get; set; } = 0;

    [JsonPropertyName("bankAccountTargetId")]
    public int BankAccountTargetId { get; set; } = 0;

    [JsonPropertyName("businessExecutiveCode")]
    public string BusinessExecutiveCode { get; set; } = string.Empty;

    [JsonPropertyName("isClosed")]
    public bool IsClosed { get; set; }

    [JsonPropertyName("isJournalPost")]
    public bool IsJournalPost { get; set; }

    [JsonPropertyName("isVoid")]
    public bool IsVoid { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [JsonPropertyName("createdOn")]
    public DateTime CreatedOn { get; set; } = new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015);

    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [JsonPropertyName("updatedOn")]
    public DateTime? UpdatedOn { get; set; } = null;

    [JsonPropertyName("updatedBy")]
    public string? UpdatedBy { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [JsonPropertyName("closedOn")]
    public DateTime? ClosedOn { get; set; } = null;

    [JsonPropertyName("closedBy")]
    public string? ClosedBy { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [JsonPropertyName("reClosedOn")]
    public DateTime? ReClosedOn { get; set; } = null;

    [JsonPropertyName("reClosedBy")]
    public string? ReClosedBy { get; set; } = null;
}