﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;
using static Xanes.Utility.SD;

namespace Xanes.Models;
[Table("currenciesexchangerates", Schema = "cnf")]
public class CurrencyExchangeRate : Entity, ICloneable
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Tipo de Registro")]
    public CurrencyType CurrencyType { get; set; } = CurrencyType.Base;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Id Moneda")]
    [ForeignKey(nameof(CurrencyTrx))]
    public int CurrencyId { get; set; }
    [ValidateNever]
    public virtual Currency CurrencyTrx { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly DateTransa { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Venta")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal SellRate { get; set; } = 0M;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Compra")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal BuyRate { get; set; } = 0M;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Oficial")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal OfficialRate { get; set; } = 0M;

    [Display(Name = "Venta Origen")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal SellRateOrigin { get; set; } = 0M;

    [Display(Name = "Compra Origen")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal BuyRateOrigin { get; set; } = 0M;

    [Display(Name = "Oficial Origen")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal OfficialRateOrigin { get; set; } = 0M;

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
            SellRateOrigin = SellRateOrigin,
            BuyRateOrigin = BuyRateOrigin,
            OfficialRateOrigin = OfficialRateOrigin,
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

