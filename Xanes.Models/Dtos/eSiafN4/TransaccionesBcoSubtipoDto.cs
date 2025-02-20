namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoSubtipoDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidRegistPad { get; set; }
    public short Numero { get; set; }
    public string Codigo { get; set; } = null!;
    public string Abreviatura { get; set; } = null!;
    public short Factor { get; set; }
    public string Descripci { get; set; } = null!;
    public string DescripciFor { get; set; } = null!;
    public long Contador { get; set; }
    public long ContadorTemporal { get; set; }
    public string FormatoContador { get; set; } = null!;
    public string FormatoContadorTemporal { get; set; } = null!;
    public short ContadorPaddingIzquierdo { get; set; }
    public short ContadorTemporalPaddingIzquierdo { get; set; }
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
}