using Xanes.Utility;

namespace Xanes.Models.ViewModels;

public class TransaODTVM : Quotation
{
    public SD.QuotationDetailType QuotationDetailType { get; set; }
    public string CustomerFullName { get; set; } = null!;
    public string NumberTransa { get; set; } = null!;
    public string CurrencySourceTarget { get; set; } = null!;
    public string BankSourceCode { get; set; } = null!;
    public string BankTargetCode { get; set; } = null!;
    public string BankAccountSourceName { get; set; } = null!;
    public string BankAccountTargetName { get; set; } = null!;
    public string ExecutiveCode { get; set; } = null!;  
    public int IdDetail { get; set; }
    public decimal ExchangeRateTransa { get; set; } = 0M;
    public decimal AmountTransactionBase { get; set; } = 0M;
    public decimal AmountTransactionForeign { get; set; } = 0M;
    public decimal AmountTransactionAdditional { get; set; } = 0M;
}