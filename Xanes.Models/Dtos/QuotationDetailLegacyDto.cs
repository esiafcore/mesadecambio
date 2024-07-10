using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class QuotationDetailLegacyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("parentId")]
    public int ParentId { get; set; }

    [JsonPropertyName("typeCode")]
    public string TypeCode { get; set; } = string.Empty;

    [JsonPropertyName("typeNumeral")]
    public int TypeNumeral { get; set; }

    [JsonPropertyName("typeDetail")]
    public int TypeDetail { get; set; }

    [JsonPropertyName("headerNumeral")]
    public int HeaderNumeral { get; set; }

    [JsonPropertyName("detailNumeral")]
    public int DetailNumeral { get; set; }

    [JsonPropertyName("currencyDetailCode")]
    public string CurrencyDetailCode { get; set; } = string.Empty;

    [JsonPropertyName("bankSourceCode")]
    public string BankSourceCode { get; set; } = string.Empty;

    [JsonPropertyName("bankAccountSourceCode")]
    public string BankAccountSourceCode { get; set; } = string.Empty;

    [JsonPropertyName("bankTargetCode")]
    public string BankTargetCode { get; set; } = string.Empty;

    [JsonPropertyName("bankAccountTargetCode")]
    public string BankAccountTargetCode { get; set; } = string.Empty;

    [JsonPropertyName("amountDetail")]
    public decimal AmountDetail { get; set; } = 0M;

    [JsonPropertyName("amountTransferFee")]
    public decimal AmountTransferFee { get; set; } = 0M;

    [JsonPropertyName("journalEntryUId")]
    public Guid? JournalEntryUId { get; set; } = null;

    [JsonPropertyName("transactionRelateUId")]
    public Guid? TransactionRelateUId { get; set; } = null;

    [JsonPropertyName("journalEntryTransferFeeId")]
    public Guid? JournalEntryTransferFeeId { get; set; } = null;

    [JsonPropertyName("bankTransactionTransferFeeId")]
    public Guid? BankTransactionTransferFeeId { get; set; } = null;
}