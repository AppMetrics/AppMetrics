// <copyright file="HistogramExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Histogram
    // ReSharper restore CheckNamespace
{
    public static class HistogramExtensions
    {
        private static readonly HistogramValue EmptyHistogram = new HistogramValue(
            0,
            0.0,
            0.0,
            null,
            0.0,
            null,
            0.0,
            0.0,
            null,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0.0,
            0);

        public static HistogramValue GetHistogramValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Histograms.ValueFor(metricName);
        }

        public static HistogramValue GetHistogramValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).Histograms.ValueFor(tags.AsMetricName(metricName));
        }

        public static HistogramValue GetValueOrDefault(this IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }
    }
}