using System.ComponentModel.DataAnnotations;
using Xanes.Utility;

namespace Xanes.Models.ViewModels;

public class TransactionReportVM
{
    [Display(Name = "Tipo de Reporte")]
    public SD.ReportTransaType ReportType { get; set; } = SD.ReportTransaType.Operation;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly DateTransaInitial { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateOnly DateTransaFinal { get; set; }
}