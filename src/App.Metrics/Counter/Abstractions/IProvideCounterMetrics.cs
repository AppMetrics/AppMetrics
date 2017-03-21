// <copyright file="IProvideCounterMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;
using App.Metrics.Tagging;

namespace App.Metrics.Counter.Abstractions
{
    public interface IProvideCounterMetrics
    {
        ICounter Instance(CounterOptions options);

        ICounter Instance<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric;

        ICounter Instance(CounterOptions options, MetricTags tags);

        ICounter Instance<T>(CounterOptions options, MetricTags tags, Func<T> builder)
            where T : ICounterMetric;
    }
}