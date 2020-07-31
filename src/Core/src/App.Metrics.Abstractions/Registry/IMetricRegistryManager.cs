// <copyright file="IMetricRegistryManager.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Registry
{
    public interface IMetricRegistryManager
    {
        IEnumerable<ApdexValueSource> ApdexScores { get; }

        IEnumerable<CounterValueSource> Counters { get; }

        IEnumerable<GaugeValueSource> Gauges { get; }

        IEnumerable<HistogramValueSource> Histograms { get; }

        IEnumerable<BucketHistogramValueSource> BucketHistograms { get; }

        IEnumerable<MeterValueSource> Meters { get; }

        IEnumerable<TimerValueSource> Timers { get; }

        IEnumerable<BucketTimerValueSource> BucketTimers { get; }
    }
}