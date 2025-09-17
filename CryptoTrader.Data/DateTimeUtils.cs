namespace CryptoTrader.Data
{
    public static class DateTimeUtils
    {
        public static DateTimeOffset StartOfMonth(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, 1, 0, 0, 0, time.Offset);
        }
        public static DateTimeOffset StartOfDay(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, 0, 0, 0, time.Offset);
        }
        public static DateTimeOffset StartOfHour(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour, 0, 0, time.Offset);
        }
        public static DateTimeOffset StartOfMinute(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Year, time.Month, time.Day, time.Hour,time.Minute, 0, time.Offset);
        }
        public static DateOnly GetDate(this DateTimeOffset time)
        {
            return DateOnly.FromDateTime(time.DateTime);
        }
        public static bool IsLatestFullHour(this DateTimeOffset time)
        {
            return time.StartOfHour() == DateTimeOffset.UtcNow.StartOfHour().AddHours(-1);
        }
        public static bool IsCurrentHour(this DateTimeOffset time)
        {
            return time.StartOfHour() == DateTimeOffset.UtcNow.StartOfHour();
        }
    }
}
