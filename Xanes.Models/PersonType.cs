using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;


namespace Xanes.Models;

[Table("personstypes", Schema = "cnf")]
public class PersonType : Entity, ICloneable
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

    public object Clone()
    {
        var obj = new PersonType
        {
            Id = Id,
            CompanyId = CompanyId,
            Numeral = Numeral,
            Code = Code,
            Name = Name
        };
        return obj;
    }
}