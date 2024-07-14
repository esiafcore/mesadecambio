namespace Xanes.Utility;

public static class AC
{
    public const string SecretUserId = "DbConnection:UserId";
    public const string SecretUserPwd = "DbConnection:Password";
    public static string Si = "Si";
    public static string No = "No";
    public static string Error = "error";
    public static string Success = "success";
    public const string CodeEmpty = "00000";
    public const int DecimalTransa = 2;
    public const int DecimalExchange = 4;
    public const int ParDecimalExchange = 4;
    public const int DecimalExchangeDouble = 8;
    public const string XlsFormatRateExchange = "#,##0.0000";
    public static char SeparationCharProperties = ',';

    public static string DefaultDateCurrent = "DateCurrent";
    public const string XlsFormatNumeric = "#,##0.00";
    public static char SeparationCharCode = '|';
    public static string Title = "Title";
    public static string DefaultDateFormatWeb = "yyyy-MM-dd";
    public static string DefaultDateFormatView = "dd/MM/yyyy";
    public const string ImagesBankFolder = @"images\banks\";
    public const string ImagesCompanyFolder = @"images\companies\";

    public const string ImagesBankSufix = "banks";
    public const string ContentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string ContentTypeZip = "application/zip";
    public const string ContentTypePdf = "application/pdf";
    public const string PaginationHeaderName = "x-pagination";
    public static string ServicesUrlApiPath = "ServicesUrl:UrlApi";
    public static string ClaimTypeRole = "role";
    public static string ClaimTypeUser = "unique_name";
    public static string UsernameLogged = "UsernameLogged";
    public static string UserEmailSession = "UserEmail";
    public const string LOCALHOSTPC = "LOCALHOSTPC";
    public const string LOCALHOSTME = "LOCALHOSTME";
    public const string Ipv4Default = "127.0.0.1";

    public const string ProcessingDate = "ProcessingDate";
    public const string ChangeProcessingDate = "ChangeProcessingDate";


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
    public static string ParIsGeneral = "parIsGeneral";
    public static string DatRep = "datrep";
    public static string ReportListData = "ListaDatosReporte";
    public static string ObjectReportData = "ObjetoReporte";
    public static string FilterReportData = "FilterReportData";
    public static string ReportDataType = "TipoReporte";
    public static string ReportDataErrorLoad = "Error al cargar el informe";
    public static string ReportFileNotFound = "No se encuentra el archivo de informe";
    public static string ReportFileInvalid = "Archivo de informe invalido";

}