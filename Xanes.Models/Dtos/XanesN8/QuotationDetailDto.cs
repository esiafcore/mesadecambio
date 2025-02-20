using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Xanes.Models.Abstractions;
using Xanes.Utility;

namespace Xanes.Models.Dtos.XanesN8;

public class QuotationDetailDto : Entity
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Parent Id")]
    public int ParentId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Tipo de Detalle")]
    public SD.QuotationDetailType QuotationDetailType { get; set; }

    [Display(Name = "Número Línea")]
    [Required(ErrorMessage = MC.RequiredMessage)]
    public int LineNumber { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Id Currency")]
    public int CurrencyDetailId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Banco Origen")]
    public int BankSourceId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Banco Origen Codigo")]
    public string BankSourceCode { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Banco Destino")]
    public int BankTargetId { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Banco Destino Codigo")]
    public string BankTargetCode { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Monto Detalle")]
    [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
    public decimal AmountDetail { get; set; } = 0M;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Porcentaje Costo Ingreso")]
    [DisplayFormat(DataFormatString = "{0:n6}", ApplyFormatInEditMode = true)]
    public decimal PercentageCostRevenue { get; set; } = 0M;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Ingreso Transacción")]
    [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
    public decimal AmountRevenue { get; set; } = 0M;

    [Required(ErrorMessage = MC.RequiredMessage)]
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

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Transaccion Bancaria Full Name")]
    public string TransactionBcoFullName { get; set; } = string.Empty;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Asiento Contable Full Name")]
    public string JournalEntryFullName { get; set; } = string.Empty;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Asiento Contable Anulado Full Name")]
    public string JournalEntryVoidFullName { get; set; } = string.Empty;
}