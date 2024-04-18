using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xanes.Models.ViewModels;

public class QuotationCreateVM
{

    public Quotation DataModel { get; set; } = null!;

    [ValidateNever]
    public IEnumerable<SelectListItem> CustomerList { get; set; } = null!;

    [ValidateNever]
    public List<Currency> CurrencyTransaList { get; set; } = new();

    [ValidateNever]
    public List<Currency> CurrencyOriginExchangeList { get; set; } = new();

    [ValidateNever]
    public List<QuotationType> QuotationTypeList { get; set; } = new();

}