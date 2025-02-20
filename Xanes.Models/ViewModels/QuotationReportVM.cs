
using System.ComponentModel.DataAnnotations;

namespace Xanes.Models.ViewModels;

public class QuotationReportVM
{
    public string CustomerFullName { get; set; } = null!;
    public string BankTargetFullName { get; set; } = null!;
    public string CurrencyTransferCode { get; set; } = null!;
    public string ConceptGeneral { get; set; } = null!;
    public string DescriptionGeneral { get; set; } = null!;
    public string NumberReferen { get; set; } = null!;  
    public decimal AmountTransaction { get; set; } = 0M;
    public bool IsClosed { get; set; }
    // Campos de agrupacion
    public int ParentQuotationId { get; set; }
    public int ParentTransactionNumber { get; set; }
    public string? ParentDateTransaFormat { get; set; }
    public string? ParentTransactionNumberFormat { get; set; }
}