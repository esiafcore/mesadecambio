using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;
namespace Xanes.Models;

[Table("bankaccounts", Schema = "bco")]
public class BankAccount : EntityInactivated, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Parent Id")]
    [ForeignKey(nameof(ParentTrx))]
    public int ParentId { get; set; }

    [ValidateNever]
    public virtual Bank ParentTrx { get; set; } = null!;

    [MaxLength(25, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name ="Código")]
    public string Code { get; set; } = null!;

    [MaxLength(75, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Es Local")]
    public bool IsLocal { get; set; } = true;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Moneda")]
    [ForeignKey(nameof(CurrencyTrx))]
    public int CurrencyId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo Moneda")]
    public SD.CurrencyType CurrencyType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyTrx { get; set; } = null!;

    #region ICloneable Members

    public virtual object Clone()
    {
        BankAccount obj = new BankAccount
        {
            Id = Id,
            CompanyId = CompanyId,
            Code = Code,
            Name = Name,
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

    #endregion


}