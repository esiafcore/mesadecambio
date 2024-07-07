using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xanes.Models.ViewModels;

public class QuotationDetailVM
{
    [ValidateNever]
    public QuotationDetail DataModel { get; set; } = null!;

    [ValidateNever]
    public string CustomerFullName { get; set; } = string.Empty;

    [ValidateNever]
    public string NumberTransa { get; set; } = string.Empty;

    [ValidateNever]
    public IEnumerable<SelectListItem> BankListItem { get; set; } = null!;
    
    [ValidateNever]
    public List<Bank> BankList { get; set; } = null!;

    [ValidateNever]
    public QuotationCreateVM ModelCreateVM { get; set; } = new();
}