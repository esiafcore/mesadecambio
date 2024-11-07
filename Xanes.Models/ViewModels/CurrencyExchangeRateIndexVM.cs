using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xanes.Utility;
// ReSharper disable InconsistentNaming

namespace Xanes.Models.ViewModels;
public class CurrencyExchangeRateIndexVM
{
    public List<CurrencyExchangeRate> DataModelList { get; set; } = null!;

    [ValidateNever]
    public IEnumerable<SelectListItem> CurrencyList { get; set; } = new List<SelectListItem>();

    [ValidateNever]
    public SD.CurrencyType CurrencySelected { get; set; }
}