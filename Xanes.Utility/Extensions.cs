namespace Xanes.Utility;
using static Xanes.Utility.SD;

public static class Extensions
{
    public static DateTime ToDateTimeConvert(this DateOnly d)
    {
        return d.ToDateTime(TimeOnly.Parse("00:00:00"));
    }

    public static DateTime GetLastDayMonth(this DateTime d)
    {
        int yearDate = d.Year;
        int monthDate = d.Month;
        int lastdayDate = DateTime.DaysInMonth(yearDate, monthDate);
        DateTime dateReturn = new DateTime(yearDate, monthDate, lastdayDate);
        return dateReturn;
    }

    public static Decimal RoundTo(this Decimal d, Int32 decimals)
    {
        return decimal
            .TryParse(d.ToString($"N{decimals}")
                , out decimal anvalueReturn) ? anvalueReturn : d;
    }

    public static Decimal ExchangeTo(this Decimal d, CurrencyType currencySource
        , CurrencyType currencyTarget
        , decimal exchangeTarget, int roundDecimal)
    {
        decimal amountExchange = 0M;

        // Base to Foreign
        if ((currencySource == CurrencyType.Base)
            && (currencyTarget == CurrencyType.Foreign)
            && (exchangeTarget != 0))
        {
            amountExchange = d / exchangeTarget;
            amountExchange = amountExchange.RoundTo(roundDecimal);
        }
        // Foreign to Base
        else if ((currencySource == CurrencyType.Foreign)
                 && (currencyTarget == CurrencyType.Base)
                 && (exchangeTarget != 0))
        {
            amountExchange = d * exchangeTarget;
            amountExchange = amountExchange.RoundTo(roundDecimal);
        }
        // Base to Additional
        else if ((currencySource == CurrencyType.Base)
                 && (currencyTarget == CurrencyType.Additional)
                 && (exchangeTarget != 0))
        {
            amountExchange = d / exchangeTarget;
            amountExchange = amountExchange.RoundTo(roundDecimal);
        }
        // Additional to Base
        else if ((currencySource == CurrencyType.Additional)
                 && (currencyTarget == CurrencyType.Base)
                 && (exchangeTarget != 0))
        {
            amountExchange = d * exchangeTarget;
            amountExchange = amountExchange.RoundTo(roundDecimal);
        }

        return amountExchange;
    }

}
