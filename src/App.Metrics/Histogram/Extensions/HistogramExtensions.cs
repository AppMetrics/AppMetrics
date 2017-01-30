// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
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

        public static HistogramValue GetValueOrDefault(this IHistogram metric)
        {
            var implementation = metric as IHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }

        public static HistogramValue GetHistogramValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Histograms.ValueFor(context, metricName);
        }

        public static IEnumerable<HistogramMetric> ToMetric(this IEnumerable<HistogramValueSource> source) { return source.Select(ToMetric); }

        public static HistogramMetric ToMetric(this HistogramValueSource source)
        {
            return new HistogramMetric
                   {
                       Name = source.Name,
                       Count = source.Value.Count,
                       Unit = source.Unit.Name,
                       LastUserValue = source.Value.LastUserValue,
                       LastValue = source.Value.LastValue,
                       Max = source.Value.Max,
                       MaxUserValue = source.Value.MaxUserValue,
                       Mean = source.Value.Mean,
                       Median = source.Value.Median,
                       Min = source.Value.Min,
                       MinUserValue = source.Value.MinUserValue,
                       Percentile75 = source.Value.Percentile75,
                       Percentile95 = source.Value.Percentile95,
                       Percentile98 = source.Value.Percentile98,
                       Percentile99 = source.Value.Percentile99,
                       Percentile999 = source.Value.Percentile999,
                       SampleSize = source.Value.SampleSize,
                       StdDev = source.Value.StdDev,
                       Tags = source.Tags
                   };
        }

        public static HistogramValueSource ToMetricValueSource(this HistogramMetric source)
        {
            var histogramValue = new HistogramValue(
                source.Count,
                source.LastValue,
                source.LastUserValue,
                source.Max,
                source.MaxUserValue,
                source.Mean,
                source.Min,
                source.MinUserValue,
                source.StdDev,
                source.Median,
                source.Percentile75,
                source.Percentile95,
                source.Percentile98,
                source.Percentile99,
                source.Percentile999,
                source.SampleSize);

            return new HistogramValueSource(source.Name, ConstantValue.Provider(histogramValue), source.Unit, source.Tags);
        }

        public static IEnumerable<HistogramValueSource> ToMetricValueSource(this IEnumerable<HistogramMetric> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}