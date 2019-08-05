// <copyright file="TimerExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Histogram;
using App.Metrics.Meter;

// ReSharper disable CheckNamespace
namespace App.Metrics.Timer
    // ReSharper restore CheckNamespace
{
    public static class BucketTimerExtensions
    {
        private static readonly BucketHistogramValue EmptyHistogram = new BucketHistogramValue(0, 0, new ReadOnlyDictionary<double, double>(new Dictionary<double, double>()));

        private static readonly MeterValue EmptyMeter = new MeterValue(0, 0.0, 0.0, 0.0, 0.0, TimeUnit.Seconds);
        private static readonly BucketTimerValue EmptyTimer = new BucketTimerValue(EmptyMeter, EmptyHistogram, 0, TimeUnit.Milliseconds);

        public static BucketTimerValue GetBucketTimerValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).BucketTimers.ValueFor(metricName);
        }

        public static BucketTimerValue GetBucketTimerValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).BucketTimers.ValueFor(tags.AsMetricName(metricName));
        }

        public static BucketTimerValue GetBucketValueOrDefault(this ITimer metric)
        {
            var implementation = metric as IBucketTimerMetric;
            return implementation != null ? implementation.Value : EmptyTimer;
        }
    }
}