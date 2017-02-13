// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
            return valueService.GetForContext(context).Histograms.ValueFor(context, metricName);
        }

        public static HistogramValue GetValueOrDefault(this IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }
    }
}