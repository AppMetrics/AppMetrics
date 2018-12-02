// <copyright file="IClock.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics
{
    public interface IClock
    {
        event EventHandler Advanced;

        long Nanoseconds { get; }

        long Seconds { get; }

        DateTime UtcDateTime { get; }

        void Advance(TimeUnit unit, long value);

        string FormatTimestamp(DateTime timestamp);
    }
}