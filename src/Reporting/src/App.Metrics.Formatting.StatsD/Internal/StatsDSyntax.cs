// <copyright file="StatsDSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Logging;

namespace App.Metrics.Formatting.StatsD.Internal
{
    public class StatsDSyntax
    {
        public static readonly string Count = "c";
        public static readonly string Gauge = "g";
        public static readonly string Histogram = "h";
        public static readonly string Timer = "ms";
        // Bug: DogStatsD does not understand meter metrics
        // Bug: StatsD removed meter metrics from their supported metrics type
        // Fix: Convert meter metrics to count
        // public static readonly string Meter = "m";

        private static readonly ILog Logger = LogProvider.GetLogger(typeof(StatsDSyntax));
        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Dictionary<Type, Func<object, string>> Formatters =
            new Dictionary<Type, Func<object, string>>
            {
                { typeof(sbyte), FormatInt },
                { typeof(byte), FormatInt },
                { typeof(short), FormatInt },
                { typeof(ushort), FormatInt },
                { typeof(int), FormatInt },
                { typeof(uint), FormatInt },
                { typeof(long), FormatInt },
                { typeof(ulong), FormatInt },
                { typeof(float), FormatFloat },
                { typeof(double), FormatFloat },
                { typeof(decimal), FormatFloat }
            };


        private static readonly Dictionary<string, string> MetricTypes =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { AppMetricsConstants.Pack.GaugeMetricTypeValue, Gauge },
                { AppMetricsConstants.Pack.CounterMetricTypeValue, Count },
                { AppMetricsConstants.Pack.HistogramMetricTypeValue, Histogram },
                { AppMetricsConstants.Pack.MeterMetricTypeValue, Count },
                { AppMetricsConstants.Pack.TimerMetricTypeValue, Timer }
            };


        // Only counters, histograms and timers can have sample rate set
        private static readonly HashSet<string> SampledTypes = new HashSet<string> { "c", "h", "ms" };

        public static bool CanBeSampled(string statsDMetricType)
            => SampledTypes.Contains(statsDMetricType);

        /// <summary>
        ///     Type of your metric either: gauge, rate, count, meter, or histogram
        /// </summary>
        public static string FormatMetricType(string metricType)
        {
            if (string.IsNullOrWhiteSpace(metricType))
            {
                return Gauge;
            }

            return MetricTypes.TryGetValue(metricType, out var type) ? type : Gauge;
        }

        /// <summary>
        ///     The numeric value format should depend on the original type. StatsD prefers integers.
        /// </summary>
        public static string FormatValue(object value, string metric)
        {
            var v = value ?? string.Empty;

            if (Formatters.TryGetValue(v.GetType(), out var format))
            {
                return format(v);
            }

            Logger.Trace(
                $"Attempted to write metric '{metric}' of type '{v.GetType()}' which is not supported. If found too noisy in logs filter in your log config or filter this from being reported, see https://www.app-metrics.io/getting-started/filtering-metrics/");

            return "0";
        }

        // Limit the floating point precision to 15
        private static string FormatFloat(object f)
            => Convert.ToDouble(f).ToString("0.###############");

        private static string FormatInt(object f)
            => Convert.ToInt64(f).ToString();
    }
}