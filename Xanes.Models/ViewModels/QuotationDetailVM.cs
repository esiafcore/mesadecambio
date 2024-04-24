using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xanes.Models.ViewModels;

public class QuotationDetailVM
{
    public Quotation DataModel { get; set; } = null!;
    public QuotationCreateVM ModelCreateVM { get; set; } = new();
}