namespace Xanes.Models.Dtos.eSiafN4;

public class CuentasBancariasDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid? UidCuentaContable { get; set; }
    public Guid UidBanco { get; set; }
    public short NumeroTipo { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripci { get; set; } = null!;
    public short NumeroMoneda { get; set; }
    public long Numero { get; set; }
    public long Contador { get; set; }
    public long ContadorTransfer { get; set; }
    public long ContadorExchange { get; set; }
    public long ContadorTemporal { get; set; }
    public long ContadorTemporalTransfer { get; set; }
    public long ContadorTemporalExchange { get; set; }
    public string FormatoContador { get; set; } = null!;
    public string FormatoContadorTemporal { get; set; } = null!;
    public short ContadorPaddingIzquierdo { get; set; }
    public short ContadorTemporalPaddingIzquierdo { get; set; } = 0;
    public string LiteralSerial { get; set; } = null!;
    public string LiteralSerialTemporal { get; set; } = null!;
    public DateTime FechaApertura { get; set; }
    public Guid? UidFormatoImpresion { get; set; }
    public decimal SaldoMonbas { get; set; }
    public decimal SaldoMonfor { get; set; }
    public decimal SaldoMonxtr { get; set; }
    public int NumeroEstado { get; set; }
    public int NumeroObjeto { get; set; }
    public bool IndUsarenCxc { get; set; }
    public bool IndUsaFormaContinua { get; set; }
    public bool IndImpresoraMatricial { get; set; }
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