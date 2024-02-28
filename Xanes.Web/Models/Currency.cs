using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Web.Abstractions;

namespace Xanes.Web.Models;

[Table("currencies", Schema = "cnf")]
public class Currency : Entity, ICloneable
{
    [StringLength(15)]
    [Required()]
    public string CodeIso { get; set; } = null!;

    [StringLength(15)]
    [Required()]
    public string Code { get; set; } = null!;

    [StringLength(5)]
    [Required()]
    public string Abbreviation { get; set; } = null!;

    [StringLength(150)]
    [Required()]
    public string Name { get; set; } = null!;
    [StringLength(150)]
    public string NameSingular { get; set; } = null!;
    [StringLength(150)]
    public string NameFor { get; set; } = null!;
    [StringLength(150)]
    public string NameForSingular { get; set; } = null!;

    [Required()]
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