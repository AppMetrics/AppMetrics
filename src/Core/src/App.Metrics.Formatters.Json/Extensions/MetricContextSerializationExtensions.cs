// <copyright file="MetricContextSerializationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
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

        public static IEnumerable<MetricsContext> ToSerializableMetric(this IEnumerable<MetricsContextValueSource> source)
        {
            return source.Select(ToSerializableMetric);
        }

        private static MetricsContextValueSource FromSerializableMetric(this MetricsContext source)
        {
            var jsonCounters = source.Counters.FromSerializableMetric();
            var jsonMeters = source.Meters.FromSerializableMetric();
            var jsonGauges = source.Gauges.FromSerializableMetric();
            var jsonHistograms = source.Histograms.FromSerializableMetric();
            var jsonBucketHistograms = source.BucketHistograms.FromSerializableMetric();
            var jsonTimers = source.Timers.FromSerializableMetric();
            var jsonBucketTimers = source.BucketTimers.FromSerializableMetric();
            var jsonApdexScores = source.ApdexScores.FromSerializableMetric();

            return new MetricsContextValueSource(source.Context, jsonGauges, jsonCounters, jsonMeters, jsonHistograms, jsonBucketHistograms, jsonTimers, jsonBucketTimers, jsonApdexScores);
        }

        private static MetricsContext ToSerializableMetric(this MetricsContextValueSource source)
        {
            var jsonCoutners = source.Counters.ToSerializableMetric();
            var jsonMeters = source.Meters.ToSerializableMetric();
            var jsonGauges = source.Gauges.ToSerializableMetric();
            var jsonHistograms = source.Histograms.ToSerializableMetric();
            var jsonBucketHistograms = source.BucketHistograms.ToSerializableMetric();
            var jsonTimers = source.Timers.ToSerializableMetric();
            var jsonBucketTimers = source.BucketTimers.ToSerializableMetric();
            var jsonApdexScores = source.ApdexScores.ToSerializableMetric();

            return new MetricsContext
                   {
                       Counters = jsonCoutners,
                       Meters = jsonMeters,
                       Gauges = jsonGauges,
                       Histograms = jsonHistograms,
                       BucketHistograms = jsonBucketHistograms,
                       Timers = jsonTimers,
                       BucketTimers = jsonBucketTimers,
                       Context = source.Context,
                       ApdexScores = jsonApdexScores
                   };
        }
    }
}