// <copyright file="PrometheusFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    public static class PrometheusFormatterConstants
    {
        public static readonly Func<string, string, string> MetricNameFormatter =
            (metricContext, metricName) => string.IsNullOrWhiteSpace(metricContext)
                ? MetricNameRegex.Replace(metricName, "_").ToLowerInvariant()
                : MetricNameRegex.Replace($"{metricContext}_{metricName}", "_").ToLowerInvariant();

        public static readonly Func<string, string> LabelNameFormatter =
            labelName => MetricNameRegex.Replace(labelName, "_").ToLowerInvariant();

        private static readonly Regex MetricNameRegex = new Regex("[^a-z0-9A-Z:_]");
    }
}