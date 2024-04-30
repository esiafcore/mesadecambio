
namespace Xanes.Models.ViewModels;

public class QuotationReportVM
{
    public string CustomerFullName { get; set; }
    public string BankTargetFullName { get; set; }
    public string CurrencyTransferCode { get; set; }
    public string ConceptGeneral { get; set; }
    public string DescriptionGeneral { get; set; }
    public string NumberReferen { get; set; }
    public decimal AmountTransaction { get; set; } = 0M;
    public bool IsClosed { get; set; }
}