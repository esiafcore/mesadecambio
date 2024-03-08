using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;


namespace Xanes.Models;

[Table("currencies", Schema = "cnf")]
public class Currency : Entity, ICloneable
{
    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Código Iso")]
    public string CodeIso { get; set; } = null!;

    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(5, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Símbolo")]
    public string Abbreviation { get; set; } = null!;

    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Nombre")]
    public string Name { get; set; } = null!;
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [DisplayName(displayName: "Nombre en Singular")]
    public string NameSingular { get; set; } = null!;
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [DisplayName(displayName: "Nombre Foráneo")]
    public string NameFor { get; set; } = null!;
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [DisplayName(displayName: "Nombre Foráneo en Singular")]
    public string NameForSingular { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Número")]
    public int Numeral { get; set; }

    public virtual object Clone()
    {
        var obj = new Currency
        {
            Id = Id,
            CompanyId = CompanyId,
            CodeIso = CodeIso,
            Code = Code,
            Abbreviation = Abbreviation,
            Name = Name,
            NameSingular = NameSingular,
            NameFor = NameFor,
            NameForSingular = NameForSingular,
            Numeral = Numeral
        };
        return obj;
    }
}