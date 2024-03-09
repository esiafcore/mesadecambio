using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;


namespace Xanes.Models;

[Table("quotationstypes", Schema = "fac")]
public class QuotationType : Entity, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Número")]
    public int Numeral { get; set; }

    [MaxLength(25, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(75, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Orden Secuencia")]
    public short OrderSequence { get; set; }

    public object Clone()
    {
        var obj = new QuotationType
        {
            Id = Id,
            CompanyId = CompanyId,
            Numeral = Numeral,
            Code = Code,
            Name = Name,
            OrderSequence = OrderSequence
        };
        return obj;
    }
}