using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;

namespace Xanes.Models;

[Table("banks", Schema = "bco")]
public class Bank : EntityInactivated, ICloneable
{
    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(75, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "% Comisión Bancaria")]
    [Range(0, 100, ErrorMessage = "Rango del campo {0} debe estar entre {1} y {2}")]
    public decimal BankingCommissionPercentage { get; set; }

    [Display(Name = "Uid Cta Bancaria Excluida")]
    public Guid? BankAccountExcludeUId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Es Compañía")]
    public bool IsCompany { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Orden Prioridad")]
    [Range(1, 100, ErrorMessage = "Rango del campo {0} debe estar entre {1} y {2}")]
    public int OrderPriority { get; set; }

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Logo URL")]
    public string? LogoUrl { get; set; }

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Logo Local Path")]
    public string? LogoLocalPath { get; set; }

    #region ICloneable Members

    public virtual object Clone()
    {
        Bank obj = new Bank
        {
            Id = Id,
            CompanyId = CompanyId,
            Code = Code,
            Name = Name,
            BankingCommissionPercentage = BankingCommissionPercentage,
            BankAccountExcludeUId = BankAccountExcludeUId,
            IsCompany = IsCompany,
            OrderPriority = OrderPriority,
            LogoUrl = LogoUrl,

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