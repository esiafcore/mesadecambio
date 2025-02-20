using System.ComponentModel.DataAnnotations;

namespace Xanes.Models.Dtos.eSiafN4;

public class ModulosDtoCreate
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public string Codigo { get; set; } = null!;
    public int Numero { get; set; }
    public string Descripci { get; set; } = null!;
    public long ConsecutivoTransa { get; set; }
    public long ConsecutivoTransaTemporal { get; set; }
    public long ConsecutivoConta { get; set; }
    public string DescripciFor { get; set; } = null!;
}