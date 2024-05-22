using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Xanes.Utility;

namespace Xanes.Models.ViewModels;

public class TransaODTVM : Quotation
{
    public SD.QuotationDetailType QuotationDetailType { get; set; }
    public string CustomerFullName { get; set; }
    public string NumberTransa { get; set; }
    public string CurrencySourceTarget { get; set; }
    public string BankSourceCode { get; set; }
    public string BankTargetCode { get; set; }
    public string ExecutiveCode { get; set; }
    public int IdDetail { get; set; }
    public decimal ExchangeRateTransa { get; set; } = 0M;
    public decimal AmountTransactionBase { get; set; } = 0M;
    public decimal AmountTransactionForeign { get; set; } = 0M;
    public decimal AmountTransactionAdditional { get; set; } = 0M;
}