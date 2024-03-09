using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Xanes.Models.Abstractions;

namespace Xanes.Models;

[Table("customers", Schema = "cxc")]
public class Customer:Entity, ICloneable
{

    [MaxLength(15, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Número")]
    public string Code { get; set; } = null!;

    [MaxLength(20, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "# Identificación")]
    public string Identificationnumber { get; set; } = null!;

    [MaxLength(250, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Primer Nombre")]
    public string FirstName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Segundo Nombre")]
    public string SecondName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Primero Apellido")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Segundo Apellido")]
    public string SecondSurname { get; set; } = null!;

    [MaxLength(550, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Razón Social")]
    public string BusinessName { get; set; } = null!;

    [MaxLength(550, ErrorMessage = "Longitud máxima del campo {0} es {1}")]
    [Display(Name = "Nombre Comercial")]
    public string CommercialName { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Categoria")]
    [ForeignKey(nameof(CategoryTrx))]
    public int CategoryId { get; set; }
    public virtual CustomerCategory CategoryTrx { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Id Tipo")]
    [ForeignKey(nameof(TypeTrx))]
    public int TypeId { get; set; }
    public virtual CustomerType TypeTrx { get; set; } = null!;

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
            Code = Code,
            Identificationnumber = Identificationnumber,
            FirstName = FirstName,
            SecondName = SecondName,
            LastName = LastName,
            SecondSurname = SecondSurname,
            BusinessName = BusinessName,
            CommercialName = CommercialName,
            CategoryId = CategoryId,
            TypeId = TypeId,
            IsBank = IsBank,
            IsSystemRow = IsSystemRow
        };
        return obj;
    }
}