namespace Extension
{
    public static class DateTimeExtensions
    {
        public static DateOnly ToDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }
        public static TimeOnly ToTimeOnly(this DateTime dateTime)
        {
            return TimeOnly.FromDateTime(dateTime);
        }
        public static DateOnly? ToNullableDateOnly(this DateTime dateTime)
        {
            return dateTime > DateTime.MinValue ? DateOnly.FromDateTime(dateTime) : null;
        }
        public static string ToNullableString(this DateTime dateTime, string format = "d")
        {            
            return dateTime > DateTime.MinValue ? dateTime.ToString(format) : null;
        }
        public static DateTime? GetNullableDateTime(this DateTime dateTime)
        {
            return dateTime > DateTime.MinValue ? dateTime : null;
        }
    }
}