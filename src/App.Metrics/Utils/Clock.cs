// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


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

        public abstract long Nanoseconds { get; }

        public abstract DateTime UtcDateTime { get; }

        public long Seconds => TimeUnit.Nanoseconds.ToSeconds(Nanoseconds);

        public string FormatTimestamp(DateTime timestamp)
        {
            return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);
        }

        public virtual void Advance(TimeUnit unit, long value)
        {
            throw new NotImplementedException($"Unable to advance {GetType()} Clock Type");
        }

        public sealed class TestClock : Clock
        {
            private long _nanoseconds = 0;

            public override long Nanoseconds => _nanoseconds;

            public override DateTime UtcDateTime => new DateTime(_nanoseconds / 100L, DateTimeKind.Utc);

            public override void Advance(TimeUnit unit, long value)
            {
                _nanoseconds += unit.ToNanoseconds(value);
                Advanced?.Invoke(this, EventArgs.Empty);
            }
        }

        private sealed class StopwatchClock : Clock
        {
            private static readonly long factor = (1000L * 1000L * 1000L) / Stopwatch.Frequency;

            public override long Nanoseconds => Stopwatch.GetTimestamp() * factor;

            public override DateTime UtcDateTime => DateTime.UtcNow;
        }

        private sealed class SystemClock : Clock
        {
            public override long Nanoseconds => DateTime.UtcNow.Ticks * 100L;

            public override DateTime UtcDateTime => DateTime.UtcNow;
        }
    }
}