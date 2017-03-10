// <copyright file="IProvideApdexMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Core.Options;
using App.Metrics.Tagging;

namespace App.Metrics.Apdex.Abstractions
{
    public interface IProvideApdexMetrics
    {
        IApdex Instance(ApdexOptions options);

        IApdex Instance(ApdexOptions options, MetricTags tags);

        IApdex Instance<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric;

        IApdex Instance<T>(ApdexOptions options, MetricTags tags, Func<T> builder)
            where T : IApdexMetric;
    }
}