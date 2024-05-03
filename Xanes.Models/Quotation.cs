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

    [Display(Name = "Cuenta Bancaria Origen")]
    [ForeignKey(nameof(BankAccountSourceTrx))]
    public int? BankAccountSourceId { get; set; }

    [ValidateNever]
    public virtual BankAccount? BankAccountSourceTrx { get; set; }

    [Display(Name = "Cuenta Bancaria Destino")]
    [ForeignKey(nameof(BankAccountTargetTrx))]
    public int? BankAccountTargetId { get; set; }
    [ValidateNever]
    public virtual BankAccount? BankAccountTargetTrx { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Deposit Currency")]
    [ForeignKey(nameof(CurrencyDepositTrx))]
    public int CurrencyDepositId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo Deposit Currency")]
    public SD.CurrencyType CurrencyDepositType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyDepositTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Transfer Currency")]
    [ForeignKey(nameof(CurrencyTransferTrx))]
    public int CurrencyTransferId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo Transfer Currency")]
    public SD.CurrencyType CurrencyTransferType { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyTransferTrx { get; set; } = null!;

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

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo de Origen de TC")]
    public SD.ExchangeRateSourceType ExchangeRateSourceType { get; set; }

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
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountTransaction { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Mesa Cambio")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountExchange { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Ingreso Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountRevenue { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Costo Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountCost { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Total Depósito")]
    [DisplayFormat(DataFormatString = "{0:n8}", ApplyFormatInEditMode = true)]
    public decimal TotalDeposit { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Total Transferencia")]
    [DisplayFormat(DataFormatString = "{0:n8}", ApplyFormatInEditMode = true)]
    public decimal TotalTransfer { get; set; }

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

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Cerrado El")]
    public DateTime ClosedDate { get; set; } = new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015);

    [Display(Name = "Cerrado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string ClosedBy { get; set; } = string.Empty;

    [Display(Name = "IPv4 Cerrado")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string ClosedIpv4 { get; set; } = string.Empty;

    [Display(Name = "HostName Cerrado")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string ClosedHostName { get; set; } = string.Empty;
    
    public object Clone()
    {
        throw new NotImplementedException();
    }
}