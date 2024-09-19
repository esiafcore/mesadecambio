using System.ComponentModel.DataAnnotations;
using Xanes.Utility;

namespace Xanes.Models.Abstractions;

public abstract class EntityInactivated
{
    public int Id { get; set; }
    public int CompanyId { get; set; }

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
}