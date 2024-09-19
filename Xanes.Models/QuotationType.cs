using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;


namespace Xanes.Models;

[Table("quotationstypes", Schema = "fac")]
public class QuotationType : Entity, ICloneable
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Número")]
    public int Numeral { get; set; }

    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(75, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
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