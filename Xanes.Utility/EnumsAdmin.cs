namespace Xanes.Utility;

public static class EnumsAdmin
{
    [Flags]
    public enum CurrencyType : short
    {
        Base = 1,
        Foreign = 2,
        Additional = 4
    }

    [Flags]
    public enum QuotationTypeNumeral : int
    {
        Buy = 1,
        Sell = 2,
        Transfer = 4,
    }

}