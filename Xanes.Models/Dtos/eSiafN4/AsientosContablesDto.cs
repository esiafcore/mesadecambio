namespace Xanes.Models.Dtos.eSiafN4;

public class AsientosContablesDto
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
    public string NumeroTransaccion { get; set; }
    public string SerieInterna { get; set; }
    public string NumeroTransaccionRef { get; set; }
    public decimal TipoCambioMonfor { get; set; }
    public decimal TipoCambioMonxtr { get; set; }
    public decimal TipoCambioparaMonfor { get; set; }
    public decimal TipoCambioparaMonxtr { get; set; }
    public short? NumeroLineas { get; set; }
    public decimal MontoDebitoMonbas { get; set; }
    public decimal MontoDebitoMonfor { get; set; }
    public decimal MontoDebitoMonxtr { get; set; }
    public decimal MontoCreditoMonbas { get; set; }
    public decimal MontoCreditoMonfor { get; set; }
    public decimal MontoCreditoMonxtr { get; set; }
    public Guid? UidProyecto { get; set; }
    public Guid? UidSucursal { get; set; }
    public string Comentarios { get; set; }
    public DateTime? FechaTransaAnula { get; set; }
    public string NumeroTransaccionAnula { get; set; }
    public string SerieInternaAnula { get; set; }
    public short? YearFiscalAnula { get; set; }
    public short? MesFiscalAnula { get; set; }
    public Guid? UidRegistref { get; set; }
    public bool IndOkay { get; set; }
    public string ComentariosSistema { get; set; }
    public DateTime CreFch { get; set; }
    public string CreUsr { get; set; }
    public string CreHsn { get; set; }
    public string CreHid { get; set; }
    public string CreIps { get; set; }
    public DateTime ModFch { get; set; }
    public string ModUsr { get; set; }
    public string ModHsn { get; set; }
    public string ModHid { get; set; }
    public string ModIps { get; set; }


}