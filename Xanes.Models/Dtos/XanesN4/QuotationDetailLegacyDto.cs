namespace Xanes.Models.Dtos.XanesN4;

public class QuotationDetailLegacyDto
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public string TypeCode { get; set; } = string.Empty;

    public int TypeNumeral { get; set; }

    public int TypeDetail { get; set; }

    public int HeaderNumeral { get; set; }

    public int DetailNumeral { get; set; }

    public string CurrencyDetailCode { get; set; } = string.Empty;

    public string BankSourceCode { get; set; } = string.Empty;

    public string BankAccountSourceCode { get; set; } = string.Empty;

    public string BankTargetCode { get; set; } = string.Empty;

    public string BankAccountTargetCode { get; set; } = string.Empty;

    public decimal AmountDetail { get; set; } = 0M;

    public decimal AmountTransferFee { get; set; } = 0M;

    public Guid? JournalEntryUId { get; set; } = null;

    public Guid? TransactionRelateUId { get; set; } = null;

    public Guid? JournalEntryTransferFeeId { get; set; } = null;

    public Guid? BankTransactionTransferFeeId { get; set; } = null;
}