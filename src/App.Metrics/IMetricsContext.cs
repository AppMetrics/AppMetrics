// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     Represents a logical grouping of metrics
    /// </summary>
    public interface IMetricsContext : IDisposable, IHideObjectMembers
    {
        IAdvancedMetricsContext Advanced { get; }

        string GroupName { get; }

        IMetricsContext Group(string groupName);

        void Decrement(CounterOptions options);

        void Decrement(CounterOptions options, long amount);

        void Decrement(CounterOptions options, string item);

        void Decrement(CounterOptions options, long amount, string item);

        void Gauge(GaugeOptions options, Func<double> valueProvider);

        void Increment(CounterOptions options);

        void Increment(CounterOptions options, long amount);

        void Increment(CounterOptions options, string item);

        void Increment(CounterOptions options, long amount, string item);

        void Mark(MeterOptions options, long amount);

        void Mark(MeterOptions options, string item);

        void Mark(MeterOptions options, long amount, string item);

        void Mark(MeterOptions options);

        void Time(TimerOptions options, Action action, string userValue = null);

        void Update(HistogramOptions options, long value, string userValue = null);

    }
}