using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
// ReSharper disable InconsistentNaming
namespace Xanes.Models.ViewModels;

public class CustomerCreateVM
{
    public Customer DataModel { get; set; } = null!;

    [ValidateNever]
    public List<PersonType> TypeList { get; set; } = new();

    [ValidateNever]
    public List<IdentificationType> IdentificationTypeList { get; set; } = new();

    //[ValidateNever]
    //public IEnumerable<SelectListItem> CategoryList { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> SectorSelectList { get; set; }

    [ValidateNever]
    public List<CustomerSector> SectorList { get; set; }

    [ValidateNever]
    public List<BusinessExecutive> BusinessExecutiveList { get; set; } = new();
}