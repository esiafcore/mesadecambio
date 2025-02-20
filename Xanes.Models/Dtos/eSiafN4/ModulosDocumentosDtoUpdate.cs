using System.ComponentModel.DataAnnotations;

namespace Xanes.Models.Dtos.eSiafN4;

public class ModulosDocumentosDtoUpdate
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidRegistPad { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripci { get; set; } = null!;
    public long ConsecutivoTransa { get; set; }
    public long ConsecutivoTransaTemporal { get; set; }
    public long ConsecutivoConta { get; set; }
    public bool IndSistema { get; set; }
    public int Numero { get; set; }
    public bool IndUsaAsientoContable { get; set; }
    public string DescripciFor { get; set; } = null!;
}