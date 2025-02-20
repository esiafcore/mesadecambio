namespace Xanes.Models.Dtos.eSiafN4;

public class AsientosContablesDtoCreate
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidModulo { get; set; }
    public Guid UidModuloDocumento { get; set; }
    public short NumeroMoneda { get; set; }
    public int NumeroEstado { get; set; }
    public int NumeroObjeto { get; set; }
    public short YearFiscal { get; set; }
    public short MesFiscal { get; set; }
    public DateTime FechaTransa { get; set; }
    public string NumeroTransaccion { get; set; } = null!;
    public string SerieInterna { get; set; } = null!;
    public string NumeroTransaccionRef { get; set; } = null!;
    public decimal TipoCambioMonfor { get; set; }
    public decimal TipoCambioMonxtr { get; set; }
    public decimal TipoCambioParaMonfor { get; set; }
    public decimal TipoCambioParaMonxtr { get; set; }
    public short? NumeroLineas { get; set; }
    public decimal MontoDebitoMonbas { get; set; }
    public decimal MontoDebitoMonfor { get; set; }
    public decimal MontoDebitoMonxtr { get; set; }
    public decimal MontoCreditoMonbas { get; set; }
    public decimal MontoCreditoMonfor { get; set; }
    public decimal MontoCreditoMonxtr { get; set; }
    public Guid? UidProyecto { get; set; }
    public Guid? UidSucursal { get; set; }
    public string Comentarios { get; set; } = null!;
    public DateTime? FechaTransaAnula { get; set; }
    public string NumeroTransaccionAnula { get; set; } = null!;
    public string SerieInternaAnula { get; set; } = null!;
    public short? YearFiscalAnula { get; set; }
    public short? MesFiscalAnula { get; set; }
    public Guid? UidRegistRef { get; set; }
    public bool IndOkay { get; set; }
    public string ComentariosSistema { get; set; } = null!;
}