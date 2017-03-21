// <copyright file="MetricsContextValueSource.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Core
{
    public sealed class MetricsContextValueSource
    {
        public static readonly MetricsContextValueSource Empty = new MetricsContextValueSource(
            string.Empty,
            Enumerable.Empty<GaugeValueSource>(),
            Enumerable.Empty<CounterValueSource>(),
            Enumerable.Empty<MeterValueSource>(),
            Enumerable.Empty<HistogramValueSource>(),
            Enumerable.Empty<TimerValueSource>(),
            Enumerable.Empty<ApdexValueSource>());

        public MetricsContextValueSource(
            string context,
            IEnumerable<GaugeValueSource> gauges,
            IEnumerable<CounterValueSource> counters,
            IEnumerable<MeterValueSource> meters,
            IEnumerable<HistogramValueSource> histograms,
            IEnumerable<TimerValueSource> timers,
            IEnumerable<ApdexValueSource> apdexScores)
        {
            Context = context;
            Gauges = gauges;
            Counters = counters;
            Meters = meters;
            Histograms = histograms;
            Timers = timers;
            ApdexScores = apdexScores;
        }

        public IEnumerable<ApdexValueSource> ApdexScores { get; }

        public string Context { get; }

        public IEnumerable<CounterValueSource> Counters { get; }

        public IEnumerable<GaugeValueSource> Gauges { get; }

        public IEnumerable<HistogramValueSource> Histograms { get; }

        public IEnumerable<MeterValueSource> Meters { get; }

        public IEnumerable<TimerValueSource> Timers { get; }

        public MetricsContextValueSource Filter(IFilterMetrics filter)
        {
            if (!filter.IsContextMatch(Context))
            {
                return Empty;
            }

            return new MetricsContextValueSource(
                Context,
                Gauges.Where(filter.IsGaugeMatch),
                Counters.Where(filter.IsCounterMatch),
                Meters.Where(filter.IsMeterMatch),
                Histograms.Where(filter.IsHistogramMatch),
                Timers.Where(filter.IsTimerMatch),
                ApdexScores.Where(filter.IsApdexMatch));
        }

        public bool IsNotEmpty() { return this != Empty; }
    }
}