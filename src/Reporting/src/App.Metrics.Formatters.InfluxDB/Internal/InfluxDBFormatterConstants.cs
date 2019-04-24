// <copyright file="InfluxDBFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    public static class InfluxDbFormatterConstants
    {
        public class LineProtocol
        {
            public static readonly Func<string, string, string> MetricNameFormatter =
                (metricContext, metricName) => string.IsNullOrWhiteSpace(metricContext)
                    ? $"{metricName}".Replace(' ', '_').ToLowerInvariant()
                    : $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();
        }
    }
}