using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using Xanes.Utility;

namespace Xanes.Models;

[Table("quotationsdetails", Schema = "fac")]
public class QuotationDetail: Entity, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Parent Id")]
    [ForeignKey(nameof(ParentTrx))]
    public int ParentId { get; set; }

    [ValidateNever]
    public virtual Quotation ParentTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Tipo de Detalle")]
    public SD.QuotationDetailType QuotationDetailType { get; set; }

    [DisplayName(displayName: "Número Línea")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    public int LineNumber { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Currency")]
    [ForeignKey(nameof(CurrencyDetailTrx))]
    public int CurrencyDetailId { get; set; }

    [ValidateNever]
    public virtual Currency CurrencyDetailTrx { get; set; } = null!;
    
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Banco Origen")]
    [ForeignKey(nameof(BankSourceTrx))]
    public int BankSourceId { get; set; }

    [ValidateNever]
    public virtual Bank BankSourceTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Banco Destino")]
    [ForeignKey(nameof(BankTargetTrx))]
    public int BankTargetId { get; set; }

    [ValidateNever]
    public virtual Bank BankTargetTrx { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Monto Detalle")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal AmountDetail { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Porcentaje Costo Ingreso")]
    [DisplayFormat(DataFormatString = "{0:n6}", ApplyFormatInEditMode = true)]
    public decimal PercentageCostRevenue { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Ingreso Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountRevenue { get; set; } = 0M;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Costo Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountCost { get; set; } = 0M;

    [Required]
    [Display(Name = "Está Contabilizado el Comprobante")]
    public bool IsJournalEntryPosted { get; set; }

    [Display(Name = "Uid Comprobante")]
    public Guid? JournalEntryId { get; set; }

    [Required]
    [Display(Name = "Está Aprobado la transacción Bancaria")]
    public bool IsBankTransactionPosted { get; set; }

    [Display(Name = "Uid Transacción")]
    public Guid? BankTransactionId { get; set; }

    [Required]
    [Display(Name = "Está Contabilizado el Comprobante")]
    public bool IsJournalEntryTransferFeePosted { get; set; }

    [Display(Name = "Uid Comprobante")]
    public Guid? JournalEntryTransferFeeId { get; set; }

    [Required]
    [Display(Name = "Está Aprobado la transacción Bancaria")]
    public bool IsBankTransactionTransferFeePosted { get; set; }

    [Display(Name = "Uid Transacción")]
    public Guid? BankTransactionTransferFeeId { get; set; }

    [Required]
    [Display(Name = "Está Contabilizado el Comprobante Anulación")]
    public bool IsJournalEntryVoidPosted { get; set; }

    [Display(Name = "Uid Comprobante Anulación")]
    public Guid? JournalEntryVoidId { get; set; }

    [Required]
    [Display(Name = "Está Aprobado la transacción Bancaria")]
    public bool IsBankTransactionVoidPosted { get; set; }

    [Display(Name = "Uid Transacción")]
    public Guid? BankTransactionVoidId { get; set; }

    [Required]
    [Display(Name = "Está Contabilizado el Comprobante Anulación")]
    public bool IsJournalEntryTransferFeeVoidPosted { get; set; }

    [Display(Name = "Uid Comprobante Anulación")]
    public Guid? JournalEntryTransferFeeVoidId { get; set; }

    [Required]
    [Display(Name = "Está Aprobado la transacción Bancaria")]
    public bool IsBankTransactionTransferFeeVoidPosted { get; set; }

    [Display(Name = "Uid Transacción")]
    public Guid? BankTransactionTransferFeeVoidId { get; set; }

    public object Clone()
    {
        throw new NotImplementedException();
    }
}