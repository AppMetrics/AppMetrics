// <copyright file="HistogramExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Histogram.Abstractions;

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

        public static HistogramValue GetValueOrDefault(this IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }
    }
}