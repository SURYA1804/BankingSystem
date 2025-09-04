public static class IndianTime
{
    public static DateTime GetIndianTime()
    {
        var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);
    }
}