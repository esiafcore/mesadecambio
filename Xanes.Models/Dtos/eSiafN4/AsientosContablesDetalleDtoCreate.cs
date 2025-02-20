namespace Xanes.Models.Dtos.eSiafN4;

public class AsientosContablesDetalleDtoCreate
{
    public Guid UidRegist { get; set; }

    public Guid UidCia { get; set; }

    public Guid UidRegistPad { get; set; }

    public Guid UidCuentaContable { get; set; }

    public Guid? UidCentroCostoContable { get; set; }

    public Guid? UidAuxiliarContable { get; set; }

    public Guid? UidCuentaPresupuesto { get; set; }

    public Guid? UidCentroCostoPresupuesto { get; set; }

    public Guid? UidActividadProyecto { get; set; }

    public Guid? UidAuxiliarPresupuesto { get; set; }

    public Guid? UidDocumento { get; set; }

    public string CodigoDocumento { get; set; } = null!;

    public int NumeroLinea { get; set; }

    public short TipoMovimiento { get; set; }

    public decimal TipoCambioMonfor { get; set; } = 0M;

    public decimal TipoCambioMonxtr { get; set; } = 0M;

    public decimal TipoCambioParaMonfor { get; set; } = 0M;

    public decimal TipoCambioParaMonxtr { get; set; } = 0M;

    public decimal MontoMonbas { get; set; } = 0M;

    public decimal MontoMonfor { get; set; } = 0M;

    public decimal MontoMonxtr { get; set; } = 0M;

    public Guid? UidBeneficiario { get; set; }

    public Guid? UidEntidad { get; set; }

    public short? TipoBeneficiario { get; set; }

    public Guid? UidCuentaBanco { get; set; }

    public Guid? UidArticulo { get; set; }

    public Guid? UidActivoFjio { get; set; }

    public Guid? UidSucursal { get; set; }

    public Guid? UidProyecto { get; set; }

    public bool IndDiferencial { get; set; }

    public string Comentarios { get; set; } = null!;

    public bool InddeCuadratura { get; set; }
}