// <copyright file="SystemClock.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using App.Metrics.Core.Internal;

namespace App.Metrics.Infrastructure
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

        public void Advance(TimeUnit unit, long value)
        {
            // DEVNOTE: Use test clock to advance the timer for testing purposes
        }

        public string FormatTimestamp(DateTime timestamp) { return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture); }
    }
}