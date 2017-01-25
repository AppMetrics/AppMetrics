// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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