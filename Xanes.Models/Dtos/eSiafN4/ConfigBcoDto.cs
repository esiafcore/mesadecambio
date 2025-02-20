namespace Xanes.Models.Dtos.eSiafN4;

public class ConfigBcoDto
{
    public Guid UidCia { get; set; }
    public Guid? CuentaContableDifPositivaConciliacion { get; set; }
    public Guid? CuentaContableDifNegativaConciliacion { get; set; }
    public Guid? CuentacontableInterfaz { get; set; }
    public Guid? CuentaContableCargaInicial { get; set; }
    public Guid? CuentaContableDepositoNoCompensado { get; set; }
    public Guid? CuentaContableDepositoContraPartida { get; set; }
    public Guid? CuentaContableComisionTransferencia { get; set; }
    public byte ConsecutivoTransaPor { get; set; }
    public byte DecimalesTransacion { get; set; }
    public byte DecimalesSaldo { get; set; }
    public byte DecimalesTipoCambio { get; set; }
    public Guid? ProveedorUnicaVez { get; set; }
    public bool IndUsaModuloContabilidad { get; set; }
    public bool IndImpresionDosFases { get; set; }
    public bool IndImprimiralGuardar { get; set; }
    public bool IndContabilizaralImprimir { get; set; }
    public bool? IndImprimirCheques { get; set; }
    public bool IndGenerarContraPartidaContableDeposito { get; set; }
    public bool IndMostrarCiudad { get; set; }
    public bool IndUsarFormatoFechaCorto { get; set; }
    public bool IndGenerarNdComisionTransferencia { get; set; }
    public bool IndUsaSolicituddePago { get; set; }
    public bool IndConsecutivoSolicitudPagoIncluyeMes { get; set; }
    public DateTime CreFch { get; set; }
    public string CreUsr { get; set; } = null!;
    public string CreHsn { get; set; } = null!;
    public string CreHid { get; set; } = null!;
    public string CreIps { get; set; } = null!;
    public DateTime ModFch { get; set; }
    public string ModUsr { get; set; } = null!;
    public string ModHsn { get; set; } = null!;
    public string ModHid { get; set; } = null!;
    public string ModIps { get; set; } = null!;
    public short? VersionFormatoImpresion { get; set; }
}