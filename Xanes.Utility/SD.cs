﻿// ReSharper disable InconsistentNaming
namespace Xanes.Utility;

public static class SD
{
    public static string SessionToken = "JWTToken";
    public const string AccessToken = "access_token";

    [Flags]
    public enum CurrencyType : int
    {
        Base = 1,
        Foreign = 2,
        Additional = 4
    }
    public enum OrderDirection
    {
        Asc,
        Desc,
    }

    [Flags]
    public enum CurrencyTypeCode : int
    {
        MonBas = 1,
        MonFor = 2,
        MonXtr = 4
    }


    [Flags]
    public enum QuotationType : int
    {
        Buy = 1,
        Sell = 2,
        Transport = 4
    }

    [Flags]
    public enum QuotationTypeName : int
    {
        Compra = 1,
        Venta = 2,
        Traslado = 4
    }

    [Flags]
    public enum QuotationTypeNameAbrv : int
    {
        Com = 1,
        Vta = 2,
        Tra = 4
    }

    [Flags]
    public enum PersonType : int
    {
        NaturalPerson = 1,
        LegalPerson = 2
    }


    [Flags]
    public enum PersonTypeCode : int
    {
        NAT = 1,
        JUR = 2
    }

    [Flags]
    public enum IdentificationTypeNumber : int
    {
        RUC = 1,
        CEDU = 2,
        DIMEX = 4,
        NITE = 8,
        DIDI = 16,
        PASS = 32
    }

    public enum CountryAlpha03 : short
    {
        CRI = 188,
        NIC = 558,
        PAN = 591,
        SLV = 222
    }

    public enum CountryAlpha02 : short
    {
        CR = 188,
        NI = 558,
        PA = 591,
        SV = 222
    }

    public enum TypeLevel : short
    {
        Root = 1,
        SubLevel = 2,
        Detail = 4
    }

    [Flags]
    public enum TypeSequential : short
    {
        Draft = 1,
        Official = 2
    }

    [Flags]
    public enum QuotationDetailType : short
    {
        Deposit = 1,
        Transfer = 2,
        CreditTransfer = 4,
        DebitTransfer = 8
    }

    public static Dictionary<short, string> QuotationDetailTypeName = new()
    {
        { (short)QuotationDetailType.Deposit,"Deposito"},
        { (short)QuotationDetailType.Transfer,"Transferencia"},
        { (short)QuotationDetailType.DebitTransfer,"Transferencia de Credito"},
        { (short)QuotationDetailType.CreditTransfer,"Transferencia de Debito"}
    };



    [Flags]
    public enum ReportTransaType : short
    {
        Operation = 1,
        Deposit = 2,
        Transfer = 4,
        Transport = 8
    }

    [Flags]
    public enum ExchangeRateSourceType : short
    {
        BaseForeign = 1,
        BaseAdditional = 2,
        ForeignAdditional = 4
    }

    public enum ParametersReport : short
    {
        DecimalBalance = 1,
        DecimalTransaction = 2,
        FilePath = 3,
        ListData = 4,
        FiltersDescription = 5,
        FileName = 6,
        ListTotal = 7
    }

    public enum MonthName : short
    {
        Inicial = 0,
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }


    public static Dictionary<short, string> SystemInformationReportTypeName = new()
    {
        // Transacciones
        { (short)ReportTransaType.Operation,"Listado de Operaciones"},
        { (short)ReportTransaType.Deposit,"Listado de Depositos"},
        { (short)ReportTransaType.Transfer,"Listado de Transferencias"},
        { (short)ReportTransaType.Transport,"Listado de Traslados"}
    };

    public static Dictionary<short, string> SystemInformationReportTypeFileName = new()
    {
        // Transacciones
        { (short)ReportTransaType.Operation,"OperationList.mrt"},
        { (short)ReportTransaType.Deposit,"DepositList.mrt"},
        { (short)ReportTransaType.Transfer,"TransferList.mrt"},
        { (short) ReportTransaType.Transport,"TransportList.mrt"}
    };

    // nombre las vistas parciales
    public static Dictionary<short, string> SystemInformationReportPartialViewName = new()
    {
        { (short)ReportTransaType.Operation,"_TransaTypeOperation"},
        { (short)ReportTransaType.Deposit,"_TransaTypeDeposit"},
        { (short)ReportTransaType.Transfer,"_TransaTypeTransfer"},
        { (short)ReportTransaType.Transport,"_TransaTypeTransport"}
    };


    /// <summary>
    /// Utilizado para saber el tipo de contenido que lleva el ACTION VERB
    /// </summary>
    public enum ContentType
    {
        Json,
        MultipartFormData,
    }
}