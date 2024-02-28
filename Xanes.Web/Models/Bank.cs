using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Web.Abstractions;
namespace Xanes.Web.Models;

[Table("banks", Schema = "bco")]
public class Bank: Entity,ICloneable
{
    [StringLength(25)]
    [Required()]
    public string Code { get; set; } = null!;

    [StringLength(75)]
    [Required()]
    public string Name { get; set; } = null!;

    [Required()]
    [Precision(18, 2)]
    public decimal ComisionBancariaPorcentaje { get; set; }

    public Guid? BankAccountExcludeUId { get; set; }

    [Required()]
    public bool IsCompany { get; set; }

    [Required()]
    public int OrderPriority { get; set; }

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
            ComisionBancariaPorcentaje = ComisionBancariaPorcentaje,
            BankAccountExcludeUId = BankAccountExcludeUId,
            IsCompany = IsCompany,
            OrderPriority = OrderPriority,
            LogoBank = LogoBank
        };
        return obj;
    }

    #endregion


}