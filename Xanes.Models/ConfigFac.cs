using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;

namespace Xanes.Models;

[Table("configsfac", Schema = "fac")]
public class ConfigFac:Entity, ICloneable
{
    [Required(ErrorMessage = "{0}, es un campo requerido.")]
    [Display(Name = "Código Transa es Automático")]
    public bool IsAutomaticallyQuotationCode { get; set; } = false;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Número de consecutivo de transaction")]
    public long SequentialNumberQuotation { get; set; } = 0;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Número Borrador de consecutivo de transacción")]
    public long SequentialNumberDraftQuotation { get; set; } = 0;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}