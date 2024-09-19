using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;


namespace Xanes.Models;

[Table("currencies", Schema = "cnf")]
public class Currency : EntityInactivated, ICloneable
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Código Iso")]
    [MaxLength(15, ErrorMessage = MC.StringLengthMessage)]
    public string CodeIso { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Código")]
    [MaxLength(15, ErrorMessage = MC.StringLengthMessage)]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Símbolo")]
    [MaxLength(5, ErrorMessage = MC.StringLengthMessage)]
    public string Abbreviation { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre")]
    [MaxLength(150, ErrorMessage = MC.StringLengthMessage)]
    public string Name { get; set; } = null!;

    [Display(Name = "Nombre en Singular")]
    [MaxLength(150, ErrorMessage = MC.StringLengthMessage)]
    public string NameSingular { get; set; } = null!;

    [Display(Name = "Nombre Foráneo")]
    [MaxLength(150, ErrorMessage = MC.StringLengthMessage)]
    public string NameForeign { get; set; } = null!;

    [Display(Name = "Nombre Foráneo en Singular")]
    [MaxLength(150, ErrorMessage = MC.StringLengthMessage)]
    public string NameForeignSingular { get; set; } = null!;

    [Display(Name = "Número")]
    [Required(ErrorMessage = MC.RequiredMessage)]
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
            NameForeign = NameForeign,
            NameForeignSingular = NameForeignSingular,
            Numeral = Numeral
        };
        return obj;
    }
}