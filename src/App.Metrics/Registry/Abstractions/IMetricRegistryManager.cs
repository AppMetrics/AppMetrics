// <copyright file="IMetricRegistryManager.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Registry.Abstractions
{
    public interface IMetricRegistryManager
    {
        IEnumerable<ApdexValueSource> ApdexScores { get; }

        IEnumerable<CounterValueSource> Counters { get; }

        IEnumerable<GaugeValueSource> Gauges { get; }

        IEnumerable<HistogramValueSource> Histograms { get; }

        IEnumerable<MeterValueSource> Meters { get; }

        IEnumerable<TimerValueSource> Timers { get; }
    }
}