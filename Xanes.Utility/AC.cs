using static System.Net.Mime.MediaTypeNames;

namespace Xanes.Utility;

public static class AC
{
    public static string Si = "Si";
    public static string No = "No";
    public const string CodeEmpty = "00000";
    public const int DecimalTransa = 2;
    public const int DecimalExchange = 4;
    public const int ParDecimalExchange = 4;
    public const int DecimalExchangeDouble = 8;
    public const string XlsFormatRateExchange = "#,##0.0000";
    public const string XlsFormatNumeric = "#,##0.00";
    public static char SeparationCharCode = '|';

    public const string ImagesBankFolder = @"images\banks\";
    public const string ImagesBankSufix = "banks";

    public const string LOCALHOSTPC = "LOCALHOSTPC";
    public const string LOCALHOSTME = "LOCALHOSTME";
    public const string Ipv4Default = "127.0.0.1";


    public static char CharDefaultEmpty = '0';
    public static short RepeatCharTimes = 5;
    public static char InternalSerialDraft = 'Y';
    public static char InternalSerialOfficial = 'Z';


    //Nombre de los parametros en los reportes
    public static string ParNameReport = "parNameReport";
    public static string ParFilterDescription = "parFilterDescription";
    public static string ParNameCompany = "parNameCompany";
    public static string ParFileImagePath = "parFileImagePath";
    public static string ParDecimalTransaction = "parDecimalTransaction";
    public static string ParDecimalBalance = "parDecimalBalance";
    public static string ParDecimalQuantity = "parDecimalQuantity";
    public static string ParDecimalUnitCost = "parDecimalUnitCost";
    public static string ParDecimalPriceSell = "parDecimalPriceSell";
    public static string ParDecimalExchangeRate = "parDecimalExchangeRate";
    public static string DatRep = "datrep";
    public static string ObjectReportData = "ObjetoReporte";

}