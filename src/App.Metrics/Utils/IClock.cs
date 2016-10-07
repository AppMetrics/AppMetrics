using System;

namespace App.Metrics.Utils
{
    public interface IClock
    {
        long Nanoseconds { get; }
        long Seconds { get; }
        DateTime UtcDateTime { get; }

        void Advance(TimeUnit unit, long value);
    }
}