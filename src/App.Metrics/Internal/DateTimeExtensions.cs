// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    internal static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime()
            .ToUniversalTime();

        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - UnixEpoch).TotalSeconds);
        }
    }
}