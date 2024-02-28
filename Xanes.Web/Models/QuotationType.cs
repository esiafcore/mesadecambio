using System.ComponentModel.DataAnnotations;
using Xanes.Web.Abstractions;

namespace Xanes.Web.Models;

public class QuotationType : Entity, ICloneable
{
    [Required()]
    public int Numeral { get; set; }

    [StringLength(25)]
    [Required()]
    public string Code { get; set; } = null!;

    [StringLength(75)]
    [Required()]
    public string Name { get; set; } = null!;

    [Required()]
    public short OrderSequence { get; set; }

    public object Clone()
    {
        var obj = new QuotationType
        {
            Id = Id,
            CompanyId = CompanyId,
            Numeral = Numeral,
            Code = Code,
            Name = Name,
            OrderSequence = OrderSequence
        };
        return obj;
    }
}