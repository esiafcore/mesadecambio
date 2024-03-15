using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Xanes.Utility.SD;

namespace Xanes.Models;

[Table("companies", Schema = "cnf")]
public class Company : ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [MaxLength(25, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    [Display(Name = "Número Identificación")]
    public string IdentificationNumber { get; set; } = null!;

    [MaxLength(250, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Nombre Comercial")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string CommercialName { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Razon Social")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string BusinessName { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [MaxLength(25, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    [Display(Name = "Codigo Pais")]
    public string CountryCode { get; set; } = null!;


    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Numero Pais")]
    public CountryAlpha03 CountryNumber { get; set; } = CountryAlpha03.NIC;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Número Telefónico")]
    [MaxLength(25, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Número de Autorización Facturación")]
    [MaxLength(50, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? BillingAuthorizationNumber { get; set; }

    [Display(Name = "Número de Autorización Contabilidad")]
    [MaxLength(50, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? JournalAuthorizationNumber { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    [Display(Name = "Dirección Principal")]
    public string AddressPrimary { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Sitio Web")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string Website { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ImageSplashUrl { get; set; }

    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ImageSplashLocalPath { get; set; }

    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ImageLogoUrl { get; set; }

    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? ImageLogoLocalPath { get; set; }

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Usa Sucursal")]
    public bool UseBranch { get; set; } = true;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Está Activo")]
    public bool IsActive { get; set; } = true;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Creado El")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "Creado Por")]
    [StringLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "IPv4 Creador")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string CreatedIpv4 { get; set; } = string.Empty;

    [Display(Name = "HostName Creador")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string CreatedHostName { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Actualizado El")]
    public DateTime? UpdatedDate { get; set; } = null;

    [Display(Name = "Actualizado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? UpdatedBy { get; set; } = null;

    [Display(Name = "IPv4 Actualizador")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? UpdatedIpv4 { get; set; } = null;

    [Display(Name = "HostName Actualizador")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? UpdatedHostName { get; set; } = null;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Inactivado El")]
    public DateTime? InactivatedDate { get; set; } = null;

    [Display(Name = "Inactivado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? InactivatedBy { get; set; } = null;

    [Display(Name = "IPv4 Inactivador")]
    [MaxLength(75, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string? InactivatedIpv4 { get; set; } = null;

    [Display(Name = "HostName Inactivador")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
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