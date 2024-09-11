namespace Xanes.Models.Dtos.eSiafN4;

public class TransaccionesBcoTipoDtoCreate
{
    public Guid UidCia { get; set; }
    public short Numero { get; set; }
    public string Codigo { get; set; }
    public string Descripci { get; set; }
    public string DescripciFor { get; set; }
    public long Contador { get; set; }
    public long ContadorTemporal { get; set; }
    public string FormatoContador { get; set; }
    public string FormatoContadorTemporal { get; set; }
    public short ContadorPaddingIzquierdo { get; set; }
    public short ContadorTemporalPaddingIzquierdo { get; set; }
}