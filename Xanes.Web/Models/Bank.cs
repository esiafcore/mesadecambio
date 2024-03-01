using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Web.Abstractions;
namespace Xanes.Web.Models;

[Table("banks", Schema = "bco")]
public class Bank: Entity,ICloneable
{
    [StringLength(25)]
    [Required()]
    [DisplayName(displayName:"Código")]
    public string Code { get; set; } = null!;

    [StringLength(75)]
    [Required()]
    [DisplayName(displayName: "Nombre")]
    public string Name { get; set; } = null!;

    [Required()]
    [Precision(18, 2)]
    [DisplayName(displayName: "% Comisión Bancaria")]
    public decimal BankingCommissionPercentage { get; set; }

    [DisplayName(displayName: "Uid Cta Bancaria Excluida")]
    public Guid? BankAccountExcludeUId { get; set; }

    [Required()]
    [DisplayName(displayName: "Es Compañía")]
    public bool IsCompany { get; set; }

    [Required()]
    [DisplayName(displayName: "Orden Prioridad")]
    public int OrderPriority { get; set; }

    [DisplayName(displayName: "Logo Banco")]
    public string? LogoBank { get; set; }

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
            LogoBank = LogoBank
        };
        return obj;
    }

    #endregion


}