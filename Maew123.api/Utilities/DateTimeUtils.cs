namespace Maew123.Api.Utilities
{
    public static class DateTimeUtils
    {
        public static DateTime UnixTimestampToDateTime(long unixTimestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
        }

        public static DateTime DateUtcToGMT7(this DateTime utcDateTime)
        {
            TimeZoneInfo gmtPlus7TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, gmtPlus7TimeZone);
        }

        public static DateTime UnixToGMT7(this DateTime utcDateTime, long unixTimestamp)
        {
            DateTime convertedUtcDateTime = UnixTimestampToDateTime(unixTimestamp);
            return convertedUtcDateTime.DateUtcToGMT7();
        }
    }
}
