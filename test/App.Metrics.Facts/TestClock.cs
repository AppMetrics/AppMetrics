using System;
using App.Metrics.Utils;

namespace App.Metrics.Facts
{
    public sealed class TestClock : Clock
    {
        private long nanoseconds = 0;

        public event EventHandler Advanced;

        public override long Nanoseconds
        {
            get { return this.nanoseconds; }
        }

        public override DateTime UTCDateTime
        {
            get { return new DateTime(this.nanoseconds / 100L, DateTimeKind.Utc); }
        }

        public void Advance(TimeUnit unit, long value)
        {
            this.nanoseconds += unit.ToNanoseconds(value);
            if (Advanced != null)
            {
                Advanced(this, EventArgs.Empty);
            }
        }
    }
}