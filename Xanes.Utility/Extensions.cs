namespace Xanes.Utility;
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

}
