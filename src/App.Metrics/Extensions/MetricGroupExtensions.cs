using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Extensions
{
    public static class MetricGroupExtensions
    {
        public static MetricGroup ToMetric(this MetricsDataGroupValueSource source)
        {
            var jsonCoutners = source.Counters.ToMetric();
            var jsonMeters = source.Meters.ToMetric();
            var jsonGauges = source.Gauges.ToMetric();
            var jsonHistograms = source.Histograms.ToMetric();
            var jsonTimers = source.Timers.ToMetric();

            return new MetricGroup
            {
                Counters = jsonCoutners,
                Meters = jsonMeters,
                Gauges = jsonGauges,
                Histograms = jsonHistograms,
                Timers = jsonTimers,
                GroupName = source.GroupName
            };
        }

        public static IEnumerable<MetricGroup> ToMetric(this IEnumerable<MetricsDataGroupValueSource> source)
        {
            return source.Select(ToMetric);
        }

        public static IEnumerable<MetricsDataGroupValueSource> ToMetricValueSource(this IEnumerable<MetricGroup> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }

        public static MetricsDataGroupValueSource ToMetricValueSource(this MetricGroup source)
        {
            var jsonCoutners = source.Counters.ToMetricValueSource();
            var jsonMeters = source.Meters.ToMetricValueSource();
            var jsonGauges = source.Gauges.ToMetricValueSource();
            var jsonHistograms = source.Histograms.ToMetricValueSource();
            var jsonTimers = source.Timers.ToMetricValueSource();

            return new MetricsDataGroupValueSource(source.GroupName, jsonGauges, jsonCoutners, jsonMeters, jsonHistograms, jsonTimers);
        }
    }
}