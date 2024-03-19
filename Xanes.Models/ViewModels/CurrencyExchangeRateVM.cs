using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
// ReSharper disable InconsistentNaming

namespace Xanes.Models.ViewModels;

public class CurrencyExchangeRateVM
{
    public CurrencyExchangeRate DataModel { get; set; } = null!;
    [ValidateNever]
    public IEnumerable<SelectListItem> CurrencyList { get; set; } = new List<SelectListItem>();

}