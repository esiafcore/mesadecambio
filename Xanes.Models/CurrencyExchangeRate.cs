using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using static Xanes.Utility.SD;

namespace Xanes.Models;
[Table("currenciesexchangerates", Schema = "cnf")]
public sealed class CurrencyExchangeRate : Entity, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo de Registro")]
    public CurrencyType CurrencyType { get; set; } = CurrencyType.Base;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Moneda")]
    [ForeignKey(nameof(CurrencyTrx))]
    public int CurrencyId { get; set; }
    [ValidateNever]
    public Currency CurrencyTrx { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly DateTransa { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Venta")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal SellRate { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Compra")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal BuyRate { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Oficial")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal OfficialRate { get; set; } = 0M;

    public object Clone()
    {
        var obj = new CurrencyExchangeRate()
        {
            Id = Id,
            CompanyId = CompanyId,
            DateTransa = DateTransa,
            SellRate = SellRate,
            OfficialRate = OfficialRate,
            BuyRate = BuyRate,
            CurrencyId = CurrencyId,
            CurrencyType = CurrencyType,

            CreatedDate = CreatedDate,
            CreatedBy = CreatedBy,
            CreatedIpv4 = CreatedIpv4,
            CreatedHostName = CreatedHostName,
            UpdatedDate = UpdatedDate,
            UpdatedBy = UpdatedBy,
            UpdatedIpv4 = UpdatedIpv4,
            UpdatedHostName = UpdatedHostName,
        };

        return obj;
    }
}

