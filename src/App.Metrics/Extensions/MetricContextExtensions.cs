// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Extensions
{
    public static class MetricContextExtensions
    {
        public static MetricsContext ToMetric(this MetricsContextValueSource source)
        {
            var jsonCoutners = source.Counters.ToMetric();
            var jsonMeters = source.Meters.ToMetric();
            var jsonGauges = source.Gauges.ToMetric();
            var jsonHistograms = source.Histograms.ToMetric();
            var jsonTimers = source.Timers.ToMetric();

            return new MetricsContext
            {
                Counters = jsonCoutners,
                Meters = jsonMeters,
                Gauges = jsonGauges,
                Histograms = jsonHistograms,
                Timers = jsonTimers,
                Context = source.Context
            };
        }

        public static IEnumerable<MetricsContext> ToMetric(this IEnumerable<MetricsContextValueSource> source)
        {
            return source.Select(ToMetric);
        }

        public static IEnumerable<MetricsContextValueSource> ToMetricValueSource(this IEnumerable<MetricsContext> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }

        public static MetricsContextValueSource ToMetricValueSource(this MetricsContext source)
        {
            var jsonCoutners = source.Counters.ToMetricValueSource();
            var jsonMeters = source.Meters.ToMetricValueSource();
            var jsonGauges = source.Gauges.ToMetricValueSource();
            var jsonHistograms = source.Histograms.ToMetricValueSource();
            var jsonTimers = source.Timers.ToMetricValueSource();

            return new MetricsContextValueSource(source.Context, jsonGauges, jsonCoutners, jsonMeters, jsonHistograms, jsonTimers);
        }
    }
}