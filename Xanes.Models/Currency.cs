﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;


namespace Xanes.Models;

[Table("currencies", Schema = "cnf")]
public class Currency : EntityInactivated, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Código Iso")]
    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string CodeIso { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Código")]
    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Símbolo")]
    [MaxLength(5, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string Abbreviation { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Nombre")]
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string Name { get; set; } = null!;

    [DisplayName(displayName: "Nombre en Singular")]
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string NameSingular { get; set; } = null!;

    [DisplayName(displayName: "Nombre Foráneo")]
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string NameForeign { get; set; } = null!;

    [DisplayName(displayName: "Nombre Foráneo en Singular")]
    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    public string NameForeignSingular { get; set; } = null!;

    [DisplayName(displayName: "Número")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
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