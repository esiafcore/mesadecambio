namespace Xanes.Models.Dtos.eSiafN4;

public class ConsecutivosCntDtoCreate
{
    public Guid UidRegist { get; set; }

    public Guid UidCia { get; set; }

    public string Categoria { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public string NombreCampo { get; set; } = null!;
        
    public long Contador { get; set; }

    public long ContadorTemporal { get; set; }

    public string FormatoContador { get; set; } = null!;

    public string FormatoContadorTemporal { get; set; } = null!;

    public short ContadorPaddingIzquierdo { get; set; }

    public short ContadorTemporalPaddingIzquierdo { get; set; }

    public bool? IndAplicar { get; set; }
}