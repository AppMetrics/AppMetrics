// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Globalization;
using App.Metrics.Abstractions.Internal;

namespace App.Metrics.Abstractions
{
    [AppMetricsExcludeFromCodeCoverage]
    public sealed class SystemClock : IClock
    {
#pragma warning disable 67
        public event EventHandler Advanced;
#pragma warning restore 67

        public long Nanoseconds => DateTime.UtcNow.Ticks * 100L;

        public long Seconds => TimeUnit.Nanoseconds.ToSeconds(Nanoseconds);

        public DateTime UtcDateTime => DateTime.UtcNow;

        public void Advance(TimeUnit unit, long value) { throw new NotImplementedException($"Unable to advance {GetType()} Clock Type"); }

        public string FormatTimestamp(DateTime timestamp) { return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture); }
    }
}