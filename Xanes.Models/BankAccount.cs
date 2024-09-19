using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;
namespace Xanes.Models;

[Table("banksaccounts", Schema = "bco")]
public class BankAccount : EntityInactivated, ICloneable
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Parent Id")]
    [ForeignKey(nameof(ParentTrx))]
    public int ParentId { get; set; }

    [ValidateNever]
    public virtual Bank ParentTrx { get; set; } = null!;

    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Número de Cuenta Bancaria")]
    public string Code { get; set; } = null!;

    [MaxLength(150, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Es Local")]
    public bool IsLocal { get; set; } = true;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Id Moneda")]
    [ForeignKey(nameof(CurrencyTrx))]
    public int CurrencyId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Tipo Moneda")]
    public SD.CurrencyType CurrencyType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyTrx { get; set; } = null!;

    [MaxLength(5, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Literal de País")]
    public string? LiteralPrefix { get; set; }

    [Display(Name = "Ledger Account Id")]
    public Guid? LedgerAccountId { get; set; }


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