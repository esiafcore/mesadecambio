
namespace Xanes.Models.Dtos.eSiafN4;

public class ConfigBcoDtoCreate
{
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
    public short? VersionFormatoImpresion { get; set; }
}