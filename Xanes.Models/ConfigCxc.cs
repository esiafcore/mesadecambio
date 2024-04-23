using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;

namespace Xanes.Models;

[Table("configscxc", Schema = "cxc")]
public class ConfigCxc: Entity, ICloneable
{
    [Required(ErrorMessage = "{0}, es un campo requerido.")]
    [Display(Name = "Código Cliente es Automático")]
    public bool IsAutomaticallyCustomerCode { get; set; } = false;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Número de consecutivo de Cliente")]
    public long SequentialNumberCustomer { get; set; } = 0;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Número Borrador de consecutivo de Cliente")]
    public long SequentialNumberDraftCustomer { get; set; } = 0;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}