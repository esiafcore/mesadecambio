namespace Xanes.Models.Dtos.eSiafN4;

public class BancosDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripci { get; set; } = null!;
    public bool IndTarjetaCredito { get; set; }
    public int NumeroObjeto { get; set; }
    public int NumeroEstado { get; set; }
    public string CodigoOperacionSwitch { get; set; } = null!;
    public Guid? CuentaContableInterfazSwitch { get; set; }
    public string CodigoOperacionMantenimiento { get; set; } = null!;
    public Guid? CuentaContableInterfazMantenimiento { get; set; }
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
    public decimal ComisionBancariaPor { get; set; }
}