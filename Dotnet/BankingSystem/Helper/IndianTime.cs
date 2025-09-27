public static class IndianTime
{
    public static DateTime GetIndianTime()
    {

    var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    var indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);
    
    // Convert back to UTC for PostgreSQL storage
    var utcTime = TimeZoneInfo.ConvertTimeToUtc(indianTime, istZone);
    return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
    }
}