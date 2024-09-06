namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoDetalleDtoUpdate
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidRegistPad { get; set; }
    public Guid UidCuentaContable { get; set; }
    public Guid? UidCentroCostoContable { get; set; }
    public Guid? UidAuxiliarContable { get; set; }
    public Guid? UidCuentaPresupuesto { get; set; }
    public Guid? UidCentroCostoPresupuesto { get; set; }
    public Guid? UidAuxiliarPresupuesto { get; set; }
    public Guid? ProyectoActividadUid { get; set; }
    public Guid? UidDocumento { get; set; }
    public string CodigoDocumento { get; set; }
    public int NumeroLinea { get; set; }
    public short TipoMovimiento { get; set; }
    public decimal TipoCambioMonfor { get; set; }
    public decimal TipoCambioMonxtr { get; set; }
    public decimal TipoCambioParaMonfor { get; set; }
    public decimal TipoCambioParaMonxtr { get; set; }
    public decimal MontoMonbas { get; set; }
    public decimal MontoMonfor { get; set; }
    public decimal MontoMonxtr { get; set; }
    public Guid? UidBeneficiario { get; set; }
    public Guid? UidEntidad { get; set; }
    public short? TipoBeneficiario { get; set; }
    public bool IndDiferencial { get; set; }
    public string Comentarios { get; set; }
    public short? TipoRegistro { get; set; }
    public bool InddeCuadratura { get; set; }
    public short NumeroTipoCambio { get; set; }
}
