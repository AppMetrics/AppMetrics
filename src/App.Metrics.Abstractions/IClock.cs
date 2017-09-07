// <copyright file="IClock.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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