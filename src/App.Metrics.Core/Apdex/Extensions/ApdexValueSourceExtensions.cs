// <copyright file="ApdexValueSourceExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Apdex
    // ReSharper restore CheckNamespace
{
    public static class ApdexValueSourceExtensions
    {
        private static readonly ApdexValue EmptyApdex = new ApdexValue(0.0, 0, 0, 0, 0);

        public static ApdexValue GetApdexValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).ApdexScores.ValueFor(metricName);
        }

        public static ApdexValue GetApdexValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).ApdexScores.ValueFor(tags.AsMetricName(metricName));
        }

        public static ApdexValue GetValueOrDefault(this IApdex metric)
        {
            var implementation = metric as IApdexMetric;
            return implementation != null ? implementation.Value : EmptyApdex;
        }
    }
}