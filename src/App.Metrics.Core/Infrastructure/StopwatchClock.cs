// <copyright file="StopwatchClock.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace App.Metrics.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public sealed class StopwatchClock : IClock
    {
        private static readonly long Factor = 1000L * 1000L * 1000L / Stopwatch.Frequency;

#pragma warning disable 67
        public event EventHandler Advanced;
#pragma warning restore 67

        public long Nanoseconds => Stopwatch.GetTimestamp() * Factor;

        public long Seconds => TimeUnit.Nanoseconds.ToSeconds(Nanoseconds);

        public DateTime UtcDateTime => DateTime.UtcNow;

        public void Advance(TimeUnit unit, long value)
        {
            // DEVNOTE: Use test clock to advance the timer for testing purposes
        }

        public string FormatTimestamp(DateTime timestamp) { return timestamp.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture); }
    }
}