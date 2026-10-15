namespace PharmacyAPI.StoreService.Services;

// "Su an acik mi" hesabi her zaman Turkiye saatiyle yapilir; sunucunun saat dilimine bakilmaz.
internal static class IstanbulClock
{
    public static DateTime Now => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Europe/Istanbul");
}
