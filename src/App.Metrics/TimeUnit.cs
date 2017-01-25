// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     The time unit used for a measurement
    /// </summary>
    public enum TimeUnit
    {
#pragma warning disable SA1602
        Nanoseconds = 0,
        Microseconds = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6
#pragma warning restore SA1602
    }
}