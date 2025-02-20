namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoSubtipoDtoCreate
{
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
}