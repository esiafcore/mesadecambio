﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
namespace Xanes.Models;

[Table("banks", Schema = "bco")]
public class Bank : Entity, ICloneable
{
    [MaxLength(25, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name ="Código")]
    public string Code { get; set; } = null!;

    [MaxLength(75, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "% Comisión Bancaria")]
    [Range(0, 100, ErrorMessage = "Rango del campo {0} debe estar entre {1} y {2}")]
    public decimal BankingCommissionPercentage { get; set; }

    [DisplayName(displayName: "Uid Cta Bancaria Excluida")]
    public Guid? BankAccountExcludeUId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Es Compañía")]
    public bool IsCompany { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DisplayName(displayName: "Orden Prioridad")]
    [Range(1, 100, ErrorMessage = "Rango del campo {0} debe estar entre {1} y {2}")]
    public int OrderPriority { get; set; }

    [MaxLength(150, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
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