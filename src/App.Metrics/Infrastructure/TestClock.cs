// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Globalization;

namespace App.Metrics.Infrastructure
{
    public sealed class TestClock : IClock
    {
        public event EventHandler Advanced;

        public long Nanoseconds { get; private set; }

        public long Seconds => TimeUnit.Nanoseconds.ToSeconds(Nanoseconds);

        public DateTime UtcDateTime => new DateTime(Nanoseconds / 100L, DateTimeKind.Utc);

        public void Advance(TimeUnit unit, long value)
        {
            Nanoseconds += unit.ToNanoseconds(value);
            Advanced?.Invoke(this, EventArgs.Empty);
        }

        public string FormatTimestamp(DateTime timestamp) { return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture); }
    }
}