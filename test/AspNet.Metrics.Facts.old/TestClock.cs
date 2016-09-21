using System;
using Metrics;
using Metrics.Utils;

namespace AspNet.Metrics.Facts
{
    public sealed class TestClock : Clock
    {
        private long _nanoseconds = 0;

        public event EventHandler Advanced;

        public override long Nanoseconds => _nanoseconds;

        public override DateTime UTCDateTime => new DateTime(this._nanoseconds / 100L, DateTimeKind.Utc);

        public void Advance(TimeUnit unit, long value)
        {
            _nanoseconds += unit.ToNanoseconds(value);
            Advanced?.Invoke(this, EventArgs.Empty);
        }
    }
}