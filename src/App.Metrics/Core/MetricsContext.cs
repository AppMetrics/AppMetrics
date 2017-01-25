// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Core
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

        public IEnumerable<MeterMetric> Meters { get; set; } = Enumerable.Empty<MeterMetric>();

        public IEnumerable<TimerMetric> Timers { get; set; } = Enumerable.Empty<TimerMetric>();
    }
}