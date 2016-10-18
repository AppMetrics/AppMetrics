using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.MetricData
{
    public sealed class MetricsData
    {
        public static readonly MetricsData Empty = new MetricsData(string.Empty, 
            DateTime.MinValue,
            Enumerable.Empty<EnvironmentInfoEntry>(),
            Enumerable.Empty<GaugeValueSource>(),
            Enumerable.Empty<CounterValueSource>(),
            Enumerable.Empty<MeterValueSource>(),
            Enumerable.Empty<HistogramValueSource>(),
            Enumerable.Empty<TimerValueSource>(),
            Enumerable.Empty<MetricsData>());

        public readonly IEnumerable<MetricsData> ChildMetrics;
        public readonly string Context;
        public readonly IEnumerable<CounterValueSource> Counters;
        public readonly IEnumerable<EnvironmentInfoEntry> Environment;
        public readonly IEnumerable<GaugeValueSource> Gauges;
        public readonly IEnumerable<HistogramValueSource> Histograms;
        public readonly IEnumerable<MeterValueSource> Meters;
        public readonly IEnumerable<TimerValueSource> Timers;
        public readonly DateTime Timestamp;

        public MetricsData(string context, DateTime timestamp,
            IEnumerable<EnvironmentInfoEntry> environment,
            IEnumerable<GaugeValueSource> gauges,
            IEnumerable<CounterValueSource> counters,
            IEnumerable<MeterValueSource> meters,
            IEnumerable<HistogramValueSource> histograms,
            IEnumerable<TimerValueSource> timers,
            IEnumerable<MetricsData> childMetrics)
        {
            Context = context;
            Timestamp = timestamp;
            Environment = environment;
            Gauges = gauges;
            Counters = counters;
            Meters = meters;
            Histograms = histograms;
            Timers = timers;
            ChildMetrics = childMetrics;
        }

        public MetricsData Filter(IMetricsFilter filter)
        {
            if (!filter.IsMatch(Context))
            {
                return Empty;
            }

            return new MetricsData(Context, Timestamp,
                Environment,
                Gauges.Where(filter.IsMatch),
                Counters.Where(filter.IsMatch),
                Meters.Where(filter.IsMatch),
                Histograms.Where(filter.IsMatch),
                Timers.Where(filter.IsMatch),
                ChildMetrics.Select(m => m.Filter(filter)));
        }

        public MetricsData Flatten()
        {
            return new MetricsData(Context, Timestamp,
                Environment.Union(ChildMetrics.SelectMany(m => m.Flatten().Environment)),
                Gauges.Union(ChildMetrics.SelectMany(m => m.Flatten().Gauges)),
                Counters.Union(ChildMetrics.SelectMany(m => m.Flatten().Counters)),
                Meters.Union(ChildMetrics.SelectMany(m => m.Flatten().Meters)),
                Histograms.Union(ChildMetrics.SelectMany(m => m.Flatten().Histograms)),
                Timers.Union(ChildMetrics.SelectMany(m => m.Flatten().Timers)),
                Enumerable.Empty<MetricsData>()
            );
        }
    }
}