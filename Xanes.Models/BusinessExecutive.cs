using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Xanes.Models.Abstractions;
using Xanes.Utility;

namespace Xanes.Models;

[Table("businessexecutives", Schema = "cxc")]
public class BusinessExecutive : EntityInactivated, ICloneable
{
    [MaxLength(15, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(250, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Primer Nombre")]
    public string FirstName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Segundo Nombre")]
    public string? SecondName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Primero Apellido")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Segundo Apellido")]
    public string? SecondSurname { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Cobranza")]
    public bool IsPayment { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Desembolso")]
    public bool IsLoan { get; set; }

    [Required]
    [Display(Name = "Por Defecto")]
    public bool IsDefault { get; set; }

    public object Clone()
    {
        BusinessExecutive obj = new BusinessExecutive
        {
            Id = Id,
            CompanyId = CompanyId,
            Code = Code,
            FirstName = FirstName,
            SecondName = SecondName,
            LastName = LastName,
            SecondSurname = SecondSurname,
            IsPayment = IsPayment,
            IsLoan = IsLoan,
            IsDefault = IsDefault,
            IsActive = IsActive,
            CreatedDate = CreatedDate,
            CreatedBy = CreatedBy,
            CreatedIpv4 = CreatedIpv4,
            CreatedHostName = CreatedHostName,
            UpdatedDate = UpdatedDate,
            UpdatedBy = UpdatedBy,
            UpdatedIpv4 = UpdatedIpv4,
            UpdatedHostName = UpdatedHostName,
            InactivatedDate = InactivatedDate,
            InactivatedBy = InactivatedBy,
            InactivatedIpv4 = InactivatedIpv4,
            InactivatedHostName = InactivatedHostName
        };
        return obj;
    }
}