namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoDtoCreate
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidBanco { get; set; }
    public Guid UidCuentaBancaria { get; set; }
    public Guid? UidCuentaBancariaRef { get; set; }
    public DateTime FechaTransa { get; set; }
    public string YearMonthFiscal { get; set; } = null!;
    public short YearFiscal { get; set; }
    public short MesFiscal { get; set; }
    public Guid UidTipo { get; set; }
    public Guid UidSubtipo { get; set; }
    public string NumeroTransaccion { get; set; } = null!;
    public string SerieInterna { get; set; } = null!;
    public string NumeroTransaccionRef { get; set; } = null!;
    public Guid? UidTransaccionRef { get; set; }
    public short NumeroMoneda { get; set; }
    public short NumeroLineas { get; set; }
    public DateTime? FechaTransaAnula { get; set; }
    public short? YearFiscalAnula { get; set; }
    public short? MesFiscalAnula { get; set; }
    public Guid? UidTransaccionAnula { get; set; }
    public decimal TipoCambioMonfor { get; set; }
    public decimal TipoCambioMonxtr { get; set; }
    public decimal TipoCambioParaMonfor { get; set; }
    public decimal TipoCambioParaMonxtr { get; set; }
    public int NumeroObjeto { get; set; }
    public int NumeroEstado { get; set; }
    public string Comentarios { get; set; } = null!;
    public bool IndOkay { get; set; }
    public decimal MontoComisionMonxtr { get; set; }
    public decimal MontoComisionMonfor { get; set; }
    public decimal MontoComisionMonbas { get; set; }
    public decimal SubtotalNetoMonbas { get; set; }
    public decimal SubtotalNetoMonfor { get; set; }
    public decimal SubtotalNetoMonxtr { get; set; }
    public decimal TotalMonbas { get; set; }
    public decimal TotalMonfor { get; set; }
    public decimal TotalMonxtr { get; set; }
    public decimal RetencionMonbas { get; set; }
    public decimal RetencionMonfor { get; set; }
    public decimal RetencionMonxtr { get; set; }
    public decimal MontoMonbas { get; set; }
    public decimal MontoMonfor { get; set; }
    public decimal MontoMonxtr { get; set; }
    public decimal MontoDebitoMonbas { get; set; }
    public decimal MontoDebitoMonfor { get; set; }
    public decimal MontoDebitoMonxtr { get; set; }
    public decimal MontoCreditoMonbas { get; set; }
    public decimal MontoCreditoMonfor { get; set; }
    public decimal MontoCreditoMonxtr { get; set; }
    public Guid? UidBeneficiario { get; set; }
    public Guid? UidEntidad { get; set; }
    public short? TipoBeneficiario { get; set; }
    public Guid? UidAsientoContable { get; set; }
    public Guid? UidAsientoContableAnula { get; set; }
    public Guid? UidSucursal { get; set; }
    public Guid? UidSolicitudPago { get; set; }
    public Guid? UidProyecto { get; set; }
    public Guid? UidDonante { get; set; }
    public bool IndConciliable { get; set; }
    public bool IndTransaccionInicial { get; set; }
    public decimal MontoAjusteConciMonbas { get; set; }
    public decimal MontoAjusteConciMonfor { get; set; }
    public decimal MontoAjusteConciMonxtr { get; set; }
    public bool IndConciliado { get; set; }
    public bool IndFlotante { get; set; }
    public Guid? UidConciliacion { get; set; }
    public bool IndCompensado { get; set; }
    public DateTime? FechaCompensacion { get; set; }
    public bool IndRetencion { get; set; }
    public Guid? UidPago { get; set; }
    public bool IndImpresoComprobante { get; set; }
    public bool IndImpresoCheque { get; set; }
    public string ImprimirChequeaNombrede { get; set; } = null!;
    public string NumeroIdentificacion { get; set; } = null!;
    public bool IndMesaDeCambio { get; set; }
    public int? TransaMcRelacionada { get; set; }
    public int? TransaMcRelacionadaParent { get; set; }
}