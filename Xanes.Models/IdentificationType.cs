using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Xanes.Models.Abstractions;

namespace Xanes.Models;
[Table("identificationstypes", Schema = "cnf")]
public class IdentificationType : EntityInactivated, ICloneable
{

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Numeral Identificación")]
    public int Numeral { get; set; }

    [MaxLength(25, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Es Juridico")]
    public bool IsLegal{ get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Es Foraneo")]
    public bool IsForeign { get; set; }

    [Display(Name = "Expresión Regular")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string RegularExpressionNumber { get; set; } = null!;

    [Display(Name = "Regex Formateo")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string FormatExpressionNumber { get; set; } = null!;

    [Display(Name = "Regex Sustitución")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string SubstitutionExpressionNumber { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Máxima Longitud Identificación")]
    public short IdentificationMaxLength { get; set; }


    public object Clone()
    {

        IdentificationType obj = new IdentificationType
        {
            Id = Id,
            CompanyId = CompanyId,
            Code = Code,
            Name = Name,
            Numeral = Numeral,
            IsLegal = IsLegal,
            IsForeign = IsForeign,
            RegularExpressionNumber = RegularExpressionNumber,
            FormatExpressionNumber = FormatExpressionNumber,
            SubstitutionExpressionNumber = SubstitutionExpressionNumber,
            IdentificationMaxLength = IdentificationMaxLength,
            IsActive = IsActive,
            CreatedDate = CreatedDate,
            CreatedBy = CreatedBy,
            CreatedIpv4 = CreatedIpv4,
            CreatedHostName = CreatedHostName,
            UpdatedDate = UpdatedDate,
            UpdatedBy = UpdatedBy,
            UpdatedIpv4 = UpdatedIpv4,
            UpdatedHostName = UpdatedHostName,
            InactivatedDate = InactivatedDate,
            InactivatedBy = InactivatedBy,
            InactivatedIpv4 = InactivatedIpv4,
            InactivatedHostName = InactivatedHostName
        };
        return obj;
    }
}