// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics.Formatters.Json
    // ReSharper restore CheckNamespace
{
    public static class MetricContextSerializationExtensions
    {
        public static IEnumerable<MetricsContextValueSource> FromSerializableMetric(this IEnumerable<MetricsContext> source)
        {
            return source.Select(x => x.FromSerializableMetric());
        }

        public static MetricsContextValueSource FromSerializableMetric(this MetricsContext source)
        {
            var jsonCounters = source.Counters.FromSerializableMetric();
            var jsonMeters = source.Meters.FromSerializableMetric();
            var jsonGauges = source.Gauges.FromSerializableMetric();
            var jsonHistograms = source.Histograms.FromSerializableMetric();
            var jsonTimers = source.Timers.FromSerializableMetric();
            var jsonApdexScores = source.ApdexScores.FromSerializableMetric();

            return new MetricsContextValueSource(source.Context, jsonGauges, jsonCounters, jsonMeters, jsonHistograms, jsonTimers, jsonApdexScores);
        }

        public static MetricsContext ToSerializableMetric(this MetricsContextValueSource source)
        {
            var jsonCoutners = source.Counters.ToSerializableMetric();
            var jsonMeters = source.Meters.ToSerializableMetric();
            var jsonGauges = source.Gauges.ToSerializableMetric();
            var jsonHistograms = source.Histograms.ToSerializableMetric();
            var jsonTimers = source.Timers.ToSerializableMetric();
            var jsonApdexScores = source.ApdexScores.ToSerializableMetric();

            return new MetricsContext
                   {
                       Counters = jsonCoutners,
                       Meters = jsonMeters,
                       Gauges = jsonGauges,
                       Histograms = jsonHistograms,
                       Timers = jsonTimers,
                       Context = source.Context,
                       ApdexScores = jsonApdexScores
                   };
        }

        public static IEnumerable<MetricsContext> ToSerializableMetric(this IEnumerable<MetricsContextValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }
    }
}