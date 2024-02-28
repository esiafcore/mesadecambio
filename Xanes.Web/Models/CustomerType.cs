using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xanes.Web.Abstractions;

namespace Xanes.Web.Models;

[Table("customerstypes", Schema = "cxc")]
public class CustomerType : Entity, ICloneable
{
    [Required()]
    public int Numeral { get; set; }

    [StringLength(25)]
    [Required()]
    public string Code { get; set; } = null!;

    [StringLength(75)]
    [Required()]
    public string Name { get; set; } = null!;

    public object Clone()
    {
        var obj = new CustomerType
        {
            Id = Id,
            CompanyId = CompanyId,
            Numeral = Numeral,
            Code = Code,
            Name = Name
        };
        return obj;
    }
}