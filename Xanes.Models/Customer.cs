using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;

namespace Xanes.Models;

[Table("customers", Schema = "cxc")]
public class Customer: EntityInactivated, ICloneable
{

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Tipo de Identificación")]
    [ForeignKey(nameof(IdentificationTypeTrx))]
    public int IdentificationTypeId { get; set; }

    [ValidateNever]
    public virtual IdentificationType IdentificationTypeTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Número Tipo de Identificación")]
    public int IdentificationTypeNumber { get; set; }

    [MaxLength(10, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Código Tipo de Identificación")]
    public string IdentificationTypeCode { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Tipo Persona")]
    [ForeignKey(nameof(TypeTrx))]
    public int TypeId { get; set; }
    [ValidateNever]
    public virtual PersonType TypeTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Número Tipo Persona")]
    public int TypeNumeral { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Sector")]
    [ForeignKey(nameof(SectorTrx))]
    public int SectorId { get; set; } = 1;

    [ValidateNever]
    public virtual CustomerSector SectorTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Internal Serial")]
    public char InternalSerial { get; set; }

    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [MaxLength(20, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "# Identificación")]
    public string IdentificationNumber { get; set; } = null!;

    [MaxLength(250, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Primer Nombre")]
    public string FirstName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Segundo Nombre")]
    public string? SecondName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Primero Apellido")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Segundo Apellido")]
    public string? SecondSurname { get; set; } = null!;

    [MaxLength(550, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Razón Social")]
    public string BusinessName { get; set; } = null!;

    [MaxLength(550, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Nombre Comercial")]
    public string CommercialName { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    [Display(Name = "Dirección Principal")]
    public string? AddressPrimary { get; set; }


    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Es Banco")]
    public bool IsBank { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Es Registro del Sistema")]
    public bool IsSystemRow { get; set; }

    public object Clone()
    {
        var obj = new Customer
        {
            Id = Id,
            CompanyId = CompanyId,
            IdentificationTypeId = IdentificationTypeId,
            IdentificationTypeNumber = IdentificationTypeNumber,
            IdentificationTypeCode = IdentificationTypeCode,
            TypeId = TypeId,
            TypeNumeral = TypeNumeral,
            Code = Code,
            IdentificationNumber = IdentificationNumber,
            FirstName = FirstName,
            SecondName = SecondName,
            LastName = LastName,
            SecondSurname = SecondSurname,
            BusinessName = BusinessName,
            CommercialName = CommercialName,
            AddressPrimary = AddressPrimary,
            IsBank = IsBank,
            IsSystemRow = IsSystemRow,

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