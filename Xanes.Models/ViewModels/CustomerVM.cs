using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
// ReSharper disable InconsistentNaming
namespace Xanes.Models.ViewModels;

public class CustomerVM
{
    public Customer DataModel { get; set; } = null!;
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> TypeList { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> SectorList { get; set; }

}