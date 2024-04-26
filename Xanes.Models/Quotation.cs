using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;

namespace Xanes.Models;
[Table("quotations", Schema = "fac")]
public class Quotation : Entity, ICloneable
{

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly DateTransa { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Type")]
    [ForeignKey(nameof(TypeTrx))]
    public int TypeId { get; set; }
    [ValidateNever]
    public virtual QuotationType TypeTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo Numeral Mesa de Cambio")]
    public SD.QuotationType TypeNumeral { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Internal Serial")]
    public char InternalSerial { get; set; }

    [DisplayName(displayName: "Número")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public int Numeral { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Cliente")]
    [ForeignKey(nameof(CustomerTrx))]
    public int CustomerId { get; set; }
    [ValidateNever]
    public virtual Customer CustomerTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id TC Source Currency")]
    [ForeignKey(nameof(CurrencyOriginExchangeTrx))]
    public int CurrencyOriginExchangeId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo TC Source Currency")]
    public SD.CurrencyType CurrencyOriginExchangeType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyOriginExchangeTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Moneda Transaction Final")]
    [ForeignKey(nameof(CurrencyTransaTrx))]
    public int CurrencyTransaId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo Moneda Transaction Final")]
    public SD.CurrencyType CurrencyTransaType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyTransaTrx { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Oficial Transa")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateOfficialTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Compra Transa")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateBuyTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Venta Transa")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateSellTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Oficial Real")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateOfficialReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Compra Real")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateBuyReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Venta Real")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal ExchangeRateSellReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Transacción")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal AmountTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Ingreso Transacción")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal AmountRevenue { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Costo Transacción")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal AmountCost { get; set; } = 0M;

    [Required]
    [Display(Name = "Está Contabilizado?")]
    public bool IsPosted { get; set; }

    [Required]
    [Display(Name = "Está Cerrado?")]
    public bool IsClosed { get; set; }

    [Required]
    [Display(Name = "Es Desembolso?")]
    public bool IsLoan { get; set; }

    [Required]
    [Display(Name = "Está Anulado?")]
    public bool IsVoid { get; set; }

    [Required]
    [Display(Name = "Es Pago?")]
    public bool IsPayment { get; set; }


    public object Clone()
    {
        throw new NotImplementedException();
    }
}