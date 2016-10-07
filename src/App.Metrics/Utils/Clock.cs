using System;
using System.Diagnostics;
using System.Globalization;

namespace App.Metrics.Utils
{
    public abstract class Clock : IClock
    {
        public static readonly Clock Default = new StopwatchClock();

        public static readonly Clock SystemDateTime = new SystemClock();

        public event EventHandler Advanced;

        public abstract long Nanoseconds { get; protected set; }

        public abstract DateTime UtcDateTime { get; }

        public long Seconds => TimeUnit.Nanoseconds.ToSeconds(Nanoseconds);

        public static string FormatTimestamp(DateTime timestamp)
        {
            return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);
        }

        public void Advance(TimeUnit unit, long value)
        {
            Nanoseconds += unit.ToNanoseconds(value);
            Advanced?.Invoke(this, EventArgs.Empty);
        }

        private sealed class StopwatchClock : Clock
        {
            private static readonly long Factor = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            private static long _nanoseconds = Stopwatch.GetTimestamp() * Factor;

            public override long Nanoseconds
            {
                get { return _nanoseconds; }
                protected set { _nanoseconds = value; }
            }

            public override DateTime UtcDateTime => DateTime.UtcNow;
        }

        private sealed class SystemClock : Clock
        {
            private static long _nanoseconds = DateTime.UtcNow.Ticks * 100L;

            public override long Nanoseconds
            {
                get { return _nanoseconds; }
                protected set { _nanoseconds = value; }
            }

            public override DateTime UtcDateTime => DateTime.UtcNow;
        }
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().ToUniversalTime();

        public static long ToUnixTime(this DateTime date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - UnixEpoch).TotalSeconds);
        }
    }
}