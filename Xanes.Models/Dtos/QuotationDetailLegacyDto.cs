using System.Text.Json.Serialization;

namespace Xanes.Models.Dtos;

public class QuotationDetailLegacyDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("parentId")]
    public int ParentId { get; set; }

    [JsonPropertyName("typeNumeral")]
    public int TypeNumeral { get; set; }

    [JsonPropertyName("typeDetail")]
    public int TypeDetail { get; set; }

    [JsonPropertyName("detailNumeral")]
    public int DetailNumeral { get; set; }

    [JsonPropertyName("bankTargetCode")]
    public string BankTargetCode { get; set; } = string.Empty;

    [JsonPropertyName("bankSourceCode")]
    public string BankSourceCode { get; set; } = string.Empty;

    [JsonPropertyName("amountDetail")]
    public decimal AmountDetail { get; set; } = 0M;

    [JsonPropertyName("journalEntryUId")]
    public Guid? JournalEntryUId { get; set; }

    [JsonPropertyName("transactionRelateUId")]
    public Guid? TransactionRelateUId { get; set; }

    [JsonPropertyName("journalEntryTransferFeeId")]
    public Guid? JournalEntryTransferFeeId { get; set; }

    [JsonPropertyName("bankTransactionTransferFeeId")]
    public Guid? BankTransactionTransferFeeId { get; set; }
}