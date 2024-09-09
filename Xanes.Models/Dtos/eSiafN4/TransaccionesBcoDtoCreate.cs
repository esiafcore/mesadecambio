namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoDtoCreate
{
    public Guid UidCia { get; set; }
    public Guid UidBanco { get; set; }
    public Guid UidCuentaBancaria { get; set; }
    public Guid? UidCuentaBancariaRef { get; set; }
    public DateTime FechaTransa { get; set; }
    public string YearMonthFiscal { get; set; } = null!;
    public short YearFiscal { get; set; } = 0;
    public short MesFiscal { get; set; } = 0;
    public Guid UidTipo { get; set; }
    public Guid UidSubtipo { get; set; }
    public string NumeroTransaccion { get; set; } = null!;
    public string SerieInterna { get; set; } = null!;
    public string NumeroTransaccionRef { get; set; } = null!;
    public Guid? UidTransaccionRef { get; set; }
    public short NumeroMoneda { get; set; } = 0;
    public short NumeroLineas { get; set; } = 0;
    public DateTime? FechaTransaAnula { get; set; }
    public short? YearFiscalAnula { get; set; }
    public short? MesFiscalAnula { get; set; }
    public Guid? UidTransaccionAnula { get; set; }
    public decimal TipoCambioMonfor { get; set; } = 0M;
    public decimal TipoCambioMonxtr { get; set; } = 0M;
    public decimal TipoCambioparaMonfor { get; set; } = 0M;
    public decimal TipoCambioparaMonxtr { get; set; } = 0M;
    public int NumeroObjeto { get; set; } = 0;
    public int NumeroEstado { get; set; } = 0;
    public string Comentarios { get; set; } = null!;
    public bool IndOkay { get; set; }
    public decimal MontoComisionMonxtr { get; set; } = 0M;
    public decimal MontoComisionMonfor { get; set; } = 0M;
    public decimal MontoComisionMonbas { get; set; } = 0M;
    public decimal SubtotalNetoMonbas { get; set; } = 0M;
    public decimal SubtotalNetoMonfor { get; set; } = 0M;
    public decimal SubtotalNetoMonxtr { get; set; } = 0M;
    public decimal TotalMonbas { get; set; } = 0M;
    public decimal TotalMonfor { get; set; } = 0M;
    public decimal TotalMonxtr { get; set; } = 0M;
    public decimal RetencionMonbas { get; set; } = 0M;
    public decimal RetencionMonfor { get; set; } = 0M;
    public decimal RetencionMonxtr { get; set; } = 0M;
    public decimal MontoMonbas { get; set; } = 0M;
    public decimal MontoMonfor { get; set; } = 0M;
    public decimal MontoMonxtr { get; set; } = 0M;
    public decimal MontoDebitoMonbas { get; set; } = 0M;
    public decimal MontoDebitoMonfor { get; set; } = 0M;
    public decimal MontoDebitoMonxtr { get; set; } = 0M;
    public decimal MontoCreditoMonbas { get; set; } = 0M;
    public decimal MontoCreditoMonfor { get; set; } = 0M;
    public decimal MontoCreditoMonxtr { get; set; } = 0M;
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
    public decimal MontoAjusteConciMonbas { get; set; } = 0M;
    public decimal MontoAjusteConciMonfor { get; set; } = 0M;
    public decimal MontoAjusteConciMonxtr { get; set; } = 0M;
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