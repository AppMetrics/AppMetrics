// <copyright file="CounterExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace App.Metrics.Counter
    // ReSharper restore CheckNamespace
{
    public static class CounterExtensions
    {
        private static readonly CounterValue EmptyCounter = new CounterValue(0);

        public static CounterValue GetCounterValue(this IProvideMetricValues valueService, string context, string metricName)
        {
            return valueService.GetForContext(context).Counters.ValueFor(metricName);
        }

        public static CounterValue GetCounterValue(this IProvideMetricValues valueService, string context, string metricName, MetricTags tags)
        {
            return valueService.GetForContext(context).Counters.ValueFor(tags.AsMetricName(metricName));
        }

        public static CounterValue GetValueOrDefault(this ICounter metric)
        {
            var implementation = metric as ICounterMetric;
            return implementation?.Value ?? EmptyCounter;
        }
    }
}