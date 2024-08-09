using Xanes.Utility;

namespace Xanes.Models.ViewModels;
public class TransportTotalVM
{
    public string BankCode { get; set; } = string.Empty;
    public decimal TotalEntryBase { get; set; } = 0M;
    public decimal TotalEntryForeign { get; set; } = 0M;
    public decimal TotalEntryAdditional { get; set; } = 0M;
    public decimal TotalOutputBase { get; set; } = 0M;
    public decimal TotalOutputForeign { get; set; } = 0M;
    public decimal TotalOutputAdditional { get; set; } = 0M;
}