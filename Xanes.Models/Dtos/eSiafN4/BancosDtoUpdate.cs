namespace Xanes.Models.Dtos.eSiafN4;

public class BancosDtoUpdate
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
    public decimal ComisionBancariaPor { get; set; }
    public DateTime? ModFch { get; set; }
    public string? ModUsr { get; set; }
    public string? ModHsn { get; set; }
    public string? ModIps { get; set; }
}