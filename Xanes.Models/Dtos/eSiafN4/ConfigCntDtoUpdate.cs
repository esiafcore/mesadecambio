namespace Xanes.Models.Dtos.eSiafN4;

public class ConfigCntDtoUpdate
{
    public Guid UidCia { get; set; }

    public bool IndContabilizarDirecto { get; set; }

    public bool IndCentroCosto { get; set; }

    public bool IndIncluirDocumentoDetalle { get; set; }

    public bool IndConsecutivoAsiento { get; set; }

    public short CuentaContableSegmentos { get; set; }

    public short CuentaContableDigitos { get; set; }

    public short CuentaContableDigitosSegmento01 { get; set; }

    public short CuentaContableDigitosSegmento02 { get; set; }

    public short CuentaContableDigitosSegmento03 { get; set; }

    public short CuentaContableDigitosSegmento04 { get; set; }

    public short CuentaContableDigitosSegmento05 { get; set; }

    public short CuentaContableDigitosSegmento06 { get; set; }

    public short CuentaContableDigitosSegmento07 { get; set; }

    public short CuentaContableDigitosSegmento08 { get; set; }

    public short CuentaContableDigitosSegmento09 { get; set; }

    public short CuentaContableDigitosSegmento10 { get; set; }

    public string CuentaContablePatronSegmento01 { get; set; } = null!;

    public string CuentaContablePatronSegmento02 { get; set; } = null!;

    public string CuentaContablePatronSegmento03 { get; set; } = null!;

    public string CuentaContablePatronSegmento04 { get; set; } = null!;

    public string CuentaContablePatronSegmento05 { get; set; } = null!;

    public string CuentaContablePatronSegmento06 { get; set; } = null!;

    public string CuentaContablePatronSegmento07 { get; set; } = null!;

    public string CuentaContablePatronSegmento08 { get; set; } = null!;

    public string CuentaContablePatronSegmento09 { get; set; } = null!;

    public string CuentaContablePatronSegmento10 { get; set; } = null!;

    public string CuentaContableExpresionRegular { get; set; } = null!;

    public string CuentaContableExpresionFormateo { get; set; } = null!;

    public string CuentaContableExpresionMascara { get; set; } = null!;

    public short CentroCostoSegmentos { get; set; }

    public short CentroCostoDigitos { get; set; }

    public short CentroCostoDigitosSegmento01 { get; set; }

    public short CentroCostoDigitosSegmento02 { get; set; }

    public short CentroCostoDigitosSegmento03 { get; set; }

    public short CentroCostoDigitosSegmento04 { get; set; }

    public short CentroCostoDigitosSegmento05 { get; set; }

    public string CentroCostoPatronSegmento01 { get; set; } = null!;

    public string CentroCostoPatronSegmento02 { get; set; } = null!;

    public string CentroCostoPatronSegmento03 { get; set; } = null!;

    public string CentroCostoPatronSegmento04 { get; set; } = null!;

    public string CentroCostoPatronSegmento05 { get; set; } = null!;

    public string CentroCostoExpresionRegular { get; set; } = null!;

    public string CentroCostoExpresionFormateo { get; set; } = null!;

    public string CentroCostoExpresionMascara { get; set; } = null!;

    public bool IndCalcularImpuestoRenta { get; set; }

    public decimal PorcentajeImpuestoRenta { get; set; }

    public Guid? ProyectoCompanyUid { get; set; }

    public decimal MontoMaximoDiferenciaContable { get; set; }

    public bool IndUsaProyecto { get; set; }

    public bool IndUsaSucursal { get; set; }

    public bool IndUsarAuxiliarContable { get; set; }

    public Guid? CuentaContableCierrePeriodoAnterior { get; set; }

    public Guid? CuentaContableCierreMesActual { get; set; }

    public Guid? CuentaContableCierreHastaMesAnterior { get; set; }

    public Guid? CuentaContableGananciaDiferencial { get; set; }

    public Guid? CuentaContablePerdidaDiferencial { get; set; }

    public Guid? CentroCostoPerdidaDiferencialUid { get; set; }

    public Guid? CentroCostoGananciaDiferencialUid { get; set; }

    public string SeparadorNivel { get; set; } = null!;

    public short NumeroFormatoImpresionComprobante { get; set; }

    public byte CharacterChasingDescripcion { get; set; }

    public short DecimalTransaccion { get; set; }

    public short DecimalSaldos { get; set; }

    public short DecimalTipoCambio { get; set; }

    public byte ConsecutivoAsientopor { get; set; }

    public Guid? CuentaContableCargaInicial { get; set; }

}