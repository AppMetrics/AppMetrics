using System;
using App.Metrics.Utils;

namespace App.Metrics.Formatters.Json.Facts.Helpers
{
    public sealed class TestClock : Clock
    {
        private readonly long _nanoseconds = 0;

        public override long Nanoseconds => _nanoseconds;

        public override DateTime UtcDateTime => new DateTime(_nanoseconds / 100L, DateTimeKind.Utc);
    }
}