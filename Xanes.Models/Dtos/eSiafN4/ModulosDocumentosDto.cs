﻿namespace Xanes.Models.Dtos.eSiafN4;

public class ModulosDocumentosDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public Guid UidRegistPad { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripci { get; set; } = null!;
    public long ConsecutivoTransa { get; set; }
    public long ConsecutivoTransaTemporal { get; set; }
    public long ConsecutivoConta { get; set; }
    public bool IndSistema { get; set; }
    public int Numero { get; set; }
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
    public bool IndUsaAsientoContable { get; set; }
    public string DescripciFor { get; set; } = null!;   
}