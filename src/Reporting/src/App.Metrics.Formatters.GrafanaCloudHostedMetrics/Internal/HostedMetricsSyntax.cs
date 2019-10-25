// <copyright file="HostedMetricsSyntax.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Logging;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal
{
    public static class HostedMetricsSyntax
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(HostedMetricsSyntax));

        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Dictionary<Type, Func<object, double>> Formatters = new Dictionary<Type, Func<object, double>>
                                                                                    {
                                                                                        { typeof(sbyte), FormatInteger },
                                                                                        { typeof(byte), FormatInteger },
                                                                                        { typeof(short), FormatInteger },
                                                                                        { typeof(ushort), FormatInteger },
                                                                                        { typeof(int), FormatInteger },
                                                                                        { typeof(uint), FormatInteger },
                                                                                        { typeof(long), FormatInteger },
                                                                                        { typeof(ulong), FormatInteger },
                                                                                        { typeof(float), FormatFloat },
                                                                                        { typeof(double), FormatFloat },
                                                                                        { typeof(decimal), FormatFloat },
                                                                                        { typeof(bool), FormatBoolean },
                                                                                        { typeof(TimeSpan), FormatTimespan }
                                                                                    };

        public static long FormatTimestamp(DateTime utcTimestamp)
        {
            return (long)utcTimestamp.Subtract(Origin).TotalSeconds;
        }

        public static double FormatValue(object value, string metric)
        {
            var v = value ?? string.Empty;

            if (Formatters.TryGetValue(v.GetType(), out Func<object, double> format))
            {
                return format(v);
            }

            Logger.Trace($"Attempted to write metric '{metric}' of type '{v.GetType()}' which is not supported. If found too noisy in logs filter in your log config or filter this from being reported, see https://www.app-metrics.io/getting-started/filtering-metrics/");

            return 0;
        }

        private static double FormatBoolean(object b) { return (bool)b ? 1 : 0; }

        private static double FormatFloat(object f) { return Convert.ToDouble(f); }

        private static double FormatInteger(object i) { return Convert.ToDouble(i); }

        private static double FormatTimespan(object ts) { return ((TimeSpan)ts).TotalMilliseconds; }
    }
}