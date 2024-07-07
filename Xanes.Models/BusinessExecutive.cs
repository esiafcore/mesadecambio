using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Models.Abstractions;

namespace Xanes.Models;

[Table("businessexecutives", Schema = "cxc")]
public class BusinessExecutive : EntityInactivated, ICloneable
{
    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Código")]
    public string Code { get; set; } = null!;

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
    
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Cobranza")]
    public bool IsPayment { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Desembolso")]
    public bool IsLoan { get; set; }

    [Required]
    [Display(Name = "Por Defecto")]
    public bool IsDefault { get; set; }

    public object Clone()
    {
        throw new NotImplementedException();
    }
}