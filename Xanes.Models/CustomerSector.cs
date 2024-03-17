﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;
using static Xanes.Utility.SD;

namespace Xanes.Models;

[Table("customerssectors", Schema = "cxc")]
public sealed class CustomerSector : EntityInactivated, ICloneable
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [MaxLength(75, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Nombre")]
    public string Name { get; set; } = null!;

    [Display(Name = "Código Ruta")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string CodePath { get; set; } = string.Empty;

    [Display(Name = "Id Ruta")]
    [MaxLength(250, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
    public string IdPath { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Profundidad")]
    public short Depthnumber { get; set; }

    [Display(Name = "Id Padre")]
    [ForeignKey(nameof(ParentTrx))]
    public int? ParentId { get; set; }
    [ValidateNever]
    public CustomerSector ParentTrx { get; set; } = null!;

    [Required(ErrorMessage = "{0} es un campo requerido.")]
    [Display(Name = "Nivel del Registro")]
    public TypeLevel TypeLevel { get; set; } = TypeLevel.Root;

    [Display(Name = "Secuencial")]
    public short? SequentialNumber { get; set; } = null;

    [Display(Name = "Secuencial Temporal")]
    public short? SequentialDraftNumber { get; set; } = null;

    public object Clone()
    {
        var obj = new CustomerSector()
        {
            Id = Id,
            CompanyId = CompanyId,
            Code = Code,
            Name = Name,
            CodePath = CodePath,
            IdPath = IdPath,
            ParentId = ParentId,
            ParentTrx = ParentTrx,
            TypeLevel = TypeLevel,
            Depthnumber = Depthnumber,
            SequentialNumber = SequentialNumber,
            SequentialDraftNumber = SequentialDraftNumber,

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
            InactivatedHostName = InactivatedHostName,
        };

        return obj;
    }

}
