// ReSharper disable InconsistentNaming
namespace Xanes.Utility;

public static class SD
{
    [Flags]
    public enum CurrencyType : int
    {
        Base = 1,
        Foreign = 2,
        Additional = 4
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
        Transfer = 4
    }

    [Flags]
    public enum PersonType : int
    {
        NaturalPerson = 1,
        LegalPerson = 2
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
    public enum TypeSequential: short
    {
        Draft = 1,
        Official = 2
    }

    [Flags]
    public enum QuotationDetailType : short
    {
        Deposit = 1,
        Transfer = 2
    }

    [Flags]
    public enum ExchangeRateSourceType : short
    {
        BaseForeign = 1,
        BaseAdditional = 2,
        ForeignAdditional = 4
    }
}