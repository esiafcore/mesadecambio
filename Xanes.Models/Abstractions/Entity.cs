﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Xanes.Models.Abstractions;

public abstract class Entity
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Display(Name = "Creado El")]
    public DateTime CreatedDate { get; set; } = new DateTime(2024, 4, 23, 0, 19, 19, 837, DateTimeKind.Utc).AddTicks(4015);

    [Display(Name = "Creado Por")]
    [MaxLength(100, ErrorMessage = "Maxima longitud para el campo {0} es {1} caracteres")]
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

}