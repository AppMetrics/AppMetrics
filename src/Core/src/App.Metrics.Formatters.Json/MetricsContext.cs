// <copyright file="MetricsContext.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     MetricsContext's are a way of grouping metrics withing a context, for example we can record all database related
    ///     metrics in a "Application.Database" Context. Metric names can be duplicated across contexts
    /// </summary>
    public sealed class MetricsContext
    {
        public IEnumerable<ApdexMetric> ApdexScores { get; set; } = Enumerable.Empty<ApdexMetric>();

        public string Context { get; set; }

        public IEnumerable<CounterMetric> Counters { get; set; } = Enumerable.Empty<CounterMetric>();

        public IEnumerable<GaugeMetric> Gauges { get; set; } = Enumerable.Empty<GaugeMetric>();

        public IEnumerable<HistogramMetric> Histograms { get; set; } = Enumerable.Empty<HistogramMetric>();

        public IEnumerable<BucketHistogramMetric> BucketHistograms { get; set; } = Enumerable.Empty<BucketHistogramMetric>();

        public IEnumerable<MeterMetric> Meters { get; set; } = Enumerable.Empty<MeterMetric>();

        public IEnumerable<TimerMetric> Timers { get; set; } = Enumerable.Empty<TimerMetric>();

        public IEnumerable<BucketTimerMetric> BucketTimers { get; set; } = Enumerable.Empty<BucketTimerMetric>();
    }
}