using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Utility;
using static Xanes.Utility.SD;

namespace Xanes.Models;

[Table("companies", Schema = "cnf")]
public class Company : ICloneable
{
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Número Identificación")]
    public string IdentificationNumber { get; set; } = null!;

    [MaxLength(250, ErrorMessage = MC.StringLengthMessage)]
    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Nombre Comercial")]
    [MaxLength(250, ErrorMessage = MC.StringLengthMessage)]
    public string CommercialName { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Razon Social")]
    [MaxLength(250, ErrorMessage = MC.StringLengthMessage)]
    public string BusinessName { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Codigo Pais")]
    public string CountryCode { get; set; } = null!;


    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Numero Pais")]
    public CountryAlpha03 CountryNumber { get; set; } = CountryAlpha03.NIC;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Número Telefónico")]
    [MaxLength(25, ErrorMessage = MC.StringLengthMessage)]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Número de Autorización Facturación")]
    [MaxLength(50, ErrorMessage = MC.StringLengthMessage)]
    public string? BillingAuthorizationNumber { get; set; }

    [Display(Name = "Número de Autorización Contabilidad")]
    [MaxLength(50, ErrorMessage = MC.StringLengthMessage)]
    public string? JournalAuthorizationNumber { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    [Display(Name = "Dirección Principal")]
    public string AddressPrimary { get; set; } = null!;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Sitio Web")]
    [MaxLength(250, ErrorMessage = MC.StringLengthMessage)]
    public string Website { get; set; } = null!;

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    public string? ImageSplashUrl { get; set; }

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    public string? ImageSplashLocalPath { get; set; }

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    public string? ImageLogoUrl { get; set; }

    [MaxLength(500, ErrorMessage = MC.StringLengthMessage)]
    public string? ImageLogoLocalPath { get; set; }

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Usa Sucursal")]
    public bool UseBranch { get; set; } = true;

    [Required(ErrorMessage = MC.RequiredMessage)]
    [Display(Name = "Está Activo")]
    public bool IsActive { get; set; } = true;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Creado El")]
    public DateTime CreatedDate { get; set; } = new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015);

    [Display(Name = "Creado Por")]
    [StringLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "IPv4 Creador")]
    [MaxLength(75, ErrorMessage = MC.StringLengthMessage)]
    public string CreatedIpv4 { get; set; } = string.Empty;

    [Display(Name = "HostName Creador")]
    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string CreatedHostName { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Actualizado El")]
    public DateTime? UpdatedDate { get; set; } = null;

    [Display(Name = "Actualizado Por")]
    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string? UpdatedBy { get; set; } = null;

    [Display(Name = "IPv4 Actualizador")]
    [MaxLength(75, ErrorMessage = MC.StringLengthMessage)]
    public string? UpdatedIpv4 { get; set; } = null;

    [Display(Name = "HostName Actualizador")]
    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string? UpdatedHostName { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Inactivado El")]
    public DateTime? InactivatedDate { get; set; } = null;

    [Display(Name = "Inactivado Por")]
    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string? InactivatedBy { get; set; } = null;

    [Display(Name = "IPv4 Inactivador")]
    [MaxLength(75, ErrorMessage = MC.StringLengthMessage)]
    public string? InactivatedIpv4 { get; set; } = null;

    [Display(Name = "HostName Inactivador")]
    [MaxLength(100, ErrorMessage = MC.StringLengthMessage)]
    public string? InactivatedHostName { get; set; } = null;

    public object Clone()
    {
        Company obj = new Company
        {
            Id = Id,
            IdentificationNumber = IdentificationNumber,
            Name = Name,
            CommercialName = CommercialName,
            BusinessName = BusinessName,
            CountryCode = CountryCode,
            CountryNumber = CountryNumber,
            PhoneNumber = PhoneNumber,
            BillingAuthorizationNumber = BillingAuthorizationNumber,
            JournalAuthorizationNumber = JournalAuthorizationNumber,
            AddressPrimary = AddressPrimary,
            Website = Website,
            ImageLogoUrl = ImageLogoUrl,
            ImageLogoLocalPath = ImageLogoLocalPath,
            ImageSplashUrl = ImageSplashUrl,
            ImageSplashLocalPath = ImageSplashLocalPath,
            UseBranch = UseBranch,
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