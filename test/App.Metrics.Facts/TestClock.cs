using System;
using App.Metrics.Utils;

namespace App.Metrics.Facts
{
    public sealed class TestClock : Clock
    {
        public override long Nanoseconds { get; protected set; } = 0;

        public override DateTime UtcDateTime => new DateTime(Nanoseconds / 100L, DateTimeKind.Utc);
    }
}