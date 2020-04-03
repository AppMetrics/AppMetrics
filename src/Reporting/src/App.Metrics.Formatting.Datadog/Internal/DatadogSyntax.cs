// <copyright file="DatadogSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Logging;

namespace App.Metrics.Formatting.Datadog.Internal
{
    /// <summary>
    /// https://docs.datadoghq.com/api/?lang=bash#post-timeseries-points
    /// </summary>
    public static class DatadogSyntax
    {
        public static readonly string Gauge = "gauge";
        public static readonly string Count = "count";
        public static readonly string Rate = "rate";

        private static readonly ILog Logger = LogProvider.GetLogger(typeof(DatadogSyntax));

        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Dictionary<Type, Func<object, double>> Formatters =
            new Dictionary<Type, Func<object, double>>
            {
                {typeof(sbyte), FormatFloat},
                {typeof(byte), FormatFloat},
                {typeof(short), FormatFloat},
                {typeof(ushort), FormatFloat},
                {typeof(int), FormatFloat},
                {typeof(uint), FormatFloat},
                {typeof(long), FormatFloat},
                {typeof(ulong), FormatFloat},
                {typeof(float), FormatFloat},
                {typeof(double), FormatFloat},
                {typeof(decimal), FormatFloat}
            };

        private static readonly Dictionary<string, string> MetricTypes =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {AppMetricsConstants.Pack.GaugeMetricTypeValue, Gauge},
                {AppMetricsConstants.Pack.CounterMetricTypeValue, Count},
                {AppMetricsConstants.Pack.MeterMetricTypeValue, Rate}
            };

        public static long FormatTimestamp(DateTime utcTimestamp)
        {
            return (long) utcTimestamp.Subtract(Origin).TotalSeconds;
        }

        /// <summary>
        /// The numeric value format should be a float value.
        /// </summary>
        public static double FormatValue(object value, string metric)
        {
            var v = value ?? string.Empty;

            if (Formatters.TryGetValue(v.GetType(), out Func<object, double> format))
            {
                return format(v);
            }

            Logger.Trace(
                $"Attempted to write metric '{metric}' of type '{v.GetType()}' which is not supported. If found too noisy in logs filter in your log config or filter this from being reported, see https://www.app-metrics.io/getting-started/filtering-metrics/");

            return 0;
        }

        /// <summary>
        /// Type of your metric either: gauge, rate, or count
        /// </summary>
        public static string FormatMetricType(string metricType)
        {
            if (string.IsNullOrWhiteSpace(metricType))
            {
                return Gauge;
            }
            
            return MetricTypes.TryGetValue(metricType, out var type) ? type : Gauge;
        }

        private static double FormatFloat(object f)
        {
            return Convert.ToDouble(f);
        }
    }
}