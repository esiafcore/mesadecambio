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
    public enum IdentificationTypeNumber : short
    {
        RUC = 1,
        CEDU = 2,
        DIMEX = 4,
        NITE = 8,
        DIDI = 16,
        PASS = 32
    }
}