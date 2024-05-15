namespace Xanes.Utility;
public static class Extensions
{
    public static DateTime ToDateTimeConvert(this DateOnly d)
    {
        return d.ToDateTime(TimeOnly.Parse("00:00:00"));
    }
}
