using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;
using static Xanes.Utility.SD;

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
    public decimal ExchangeRateOfficialTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Compra Transa")]
    public decimal ExchangeRateBuyTransa { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Venta Transa")]
    public decimal ExchangeRateSellTransa { get; set; } = 0M;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo de Origen de TC")]
    public SD.ExchangeRateSourceType ExchangeRateSourceType { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Oficial Real")]
    public decimal ExchangeRateOfficialReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Compra Real")]
    public decimal ExchangeRateBuyReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "TC Venta Real")]
    public decimal ExchangeRateSellReal { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountTransaction { get; set; } = 0M;
    [NotMapped]
    public decimal AmountTransactionRpt
    {
        get
        {
            var total = this.AmountTransaction;

            if ((!this.IsClosed) || (this.IsVoid))
            {
                total = 0M;
            }

            return total;
        }
    }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Comisión TRF")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountCommission { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Mesa Cambio")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountExchange { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Ingreso Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountRevenue { get; set; } = 0M;
    [NotMapped]
    public decimal AmountRevenueRpt
    {
        get
        {
            var total = this.AmountRevenue;

            if ((!this.IsClosed) || (this.IsVoid))
            {
                total = 0M;
            }

            return total;
        }
    }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Costo Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountCost { get; set; } = 0M;
    [NotMapped]
    public decimal AmountCostRpt
    {
        get
        {
            var total = this.AmountCost;

            if ((!this.IsClosed) || (this.IsVoid))
            {
                total = 0M;
            }

            return total;
        }
    }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Total Depósito")]
    [DisplayFormat(DataFormatString = "{0:n8}", ApplyFormatInEditMode = true)]
    public decimal TotalDeposit { get; set; } = 0M;
    [NotMapped]
    public decimal TotalDepositRpt
    {
        get
        {
            var total = this.TotalDeposit;

            if ((!this.IsClosed) || (this.IsVoid))
            {
                total = 0M;
            }

            return total;
        }
    }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Total Transferencia")]
    [DisplayFormat(DataFormatString = "{0:n8}", ApplyFormatInEditMode = true)]
    public decimal TotalTransfer { get; set; } = 0M;
    [NotMapped]
    public decimal TotalTransferRpt
    {
        get
        {
            var total = this.TotalTransfer;

            if ((!this.IsClosed) || (this.IsVoid))
            {
                total = 0M;
            }

            return total;
        }
    }

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

    [Required]
    [Display(Name = "Está ajustado")]
    public bool IsAdjustment { get; set; } = false;

    [Required]
    [Display(Name = "Es Banco")]
    public bool IsBank { get; set; } = false;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id BusinessExecutive")]
    [ForeignKey(nameof(BusinessExecutiveTrx))]
    public int BusinessExecutiveId { get; set; }
    [ValidateNever]
    public virtual BusinessExecutive BusinessExecutiveTrx { get; set; } = null!;

    [Display(Name = "Ejecutivo Código")]
    public string BusinessExecutiveCode { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Cerrado El")]
    public DateTime? ClosedDate { get; set; } = null;

    [Display(Name = "Cerrado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ClosedBy { get; set; } = null;

    [Display(Name = "IPv4 Cerrado")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ClosedIpv4 { get; set; } = null;

    [Display(Name = "HostName Cerrado")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ClosedHostName { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "ReCerrado El")]
    public DateTime? ReClosedDate { get; set; } = null;

    [Display(Name = "ReCerrado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ReClosedBy { get; set; } = null;

    [Display(Name = "IPv4 ReCerrado")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ReClosedIpv4 { get; set; } = null;

    [Display(Name = "HostName ReCerrado")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ReClosedHostName { get; set; } = null;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}