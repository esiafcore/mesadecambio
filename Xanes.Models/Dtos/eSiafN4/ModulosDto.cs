﻿using System.ComponentModel.DataAnnotations;

namespace Xanes.Models.Dtos.eSiafN4;

public class ModulosDto
{
    public Guid UidRegist { get; set; }
    public Guid UidCia { get; set; }
    public string Codigo { get; set; }
    public int Numero { get; set; }
    public string Descripci { get; set; }
    public long ConsecutivoTransa { get; set; }
    public long ConsecutivoTransaTemporal { get; set; }
    public long ConsecutivoConta { get; set; }
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
    public string DescripciFor { get; set; }
}