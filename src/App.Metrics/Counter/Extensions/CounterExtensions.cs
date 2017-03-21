// <copyright file="CounterExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter.Abstractions;

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

        public static CounterValue GetValueOrDefault(this ICounter metric)
        {
            var implementation = metric as ICounterMetric;
            return implementation?.Value ?? EmptyCounter;
        }
    }
}