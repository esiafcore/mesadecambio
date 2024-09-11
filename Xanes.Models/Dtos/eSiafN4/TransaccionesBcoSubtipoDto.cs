namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoSubtipoDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidRegistPad { get; set; }
    public short Numero { get; set; }
    public string Codigo { get; set; }
    public string Abreviatura { get; set; }
    public short Factor { get; set; }
    public string Descripci { get; set; }
    public string DescripciFor { get; set; }
    public long Contador { get; set; }
    public long ContadorTemporal { get; set; }
    public string FormatoContador { get; set; }
    public string FormatoContadorTemporal { get; set; }
    public short ContadorPaddingIzquierdo { get; set; }
    public short ContadorTemporalPaddingIzquierdo { get; set; }
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