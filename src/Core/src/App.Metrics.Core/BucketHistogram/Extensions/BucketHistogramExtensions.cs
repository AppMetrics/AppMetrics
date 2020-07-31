// <copyright file="HistogramExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.BucketHistogram;

namespace App.Metrics.Histogram
    // ReSharper restore CheckNamespace
{
    public static class BucketHistogramExtensions
    {
        private static readonly BucketHistogramValue EmptyHistogram = new BucketHistogramValue(0,0, new ReadOnlyDictionary<double, double>(new Dictionary<double, double>()));

        public static BucketHistogramValue GetBucketHistogramValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).BucketHistograms.ValueFor(metricName);
        }

        public static BucketHistogramValue GetBucketHistogramValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).BucketHistograms.ValueFor(tags.AsMetricName(metricName));
        }

        public static BucketHistogramValue GetValueOrDefault(this IBucketHistogram metric)
        {
            var implementation = metric as IBucketHistogramMetric;
            return implementation != null ? implementation.Value : EmptyHistogram;
        }
    }
}