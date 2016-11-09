using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Data
{
    public sealed class MetricsContextValueSource
    {
        public static readonly MetricsContextValueSource Empty = new MetricsContextValueSource(string.Empty,
            Enumerable.Empty<GaugeValueSource>(),
            Enumerable.Empty<CounterValueSource>(),
            Enumerable.Empty<MeterValueSource>(),
            Enumerable.Empty<HistogramValueSource>(),
            Enumerable.Empty<TimerValueSource>());

        public readonly IEnumerable<CounterValueSource> Counters;
        public readonly IEnumerable<GaugeValueSource> Gauges;

        public readonly string Context;
        public readonly IEnumerable<HistogramValueSource> Histograms;
        public readonly IEnumerable<MeterValueSource> Meters;
        public readonly IEnumerable<TimerValueSource> Timers;

        public MetricsContextValueSource(string context,
            IEnumerable<GaugeValueSource> gauges,
            IEnumerable<CounterValueSource> counters,
            IEnumerable<MeterValueSource> meters,
            IEnumerable<HistogramValueSource> histograms,
            IEnumerable<TimerValueSource> timers)
        {
            Context = context;
            Gauges = gauges;
            Counters = counters;
            Meters = meters;
            Histograms = histograms;
            Timers = timers;
        }

        public bool IsNotEmpty()
        {
            return this != Empty;
        }

        public MetricsContextValueSource Filter(IMetricsFilter filter)
        {
            if (!filter.IsMatch(Context))
            {
                return Empty;
            }

            return new MetricsContextValueSource(Context,
                Gauges.Where(filter.IsMatch),
                Counters.Where(filter.IsMatch),
                Meters.Where(filter.IsMatch),
                Histograms.Where(filter.IsMatch),
                Timers.Where(filter.IsMatch));
        }
    }
}