// <copyright file="ApdexMetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class ApdexMetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        public static IHealthCheckRegistry AddMetricCheck(
            this IHealthCheckRegistry registry,
            string name,
            IMetrics metrics,
            ApdexOptions options,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            return registry.AddMetricCheck(name, metrics, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckRegistry AddMetricCheck(
            this IHealthCheckRegistry registry,
            string name,
            IMetrics metrics,
            ApdexOptions options,
            MetricTags tags,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            registry.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? metrics.Snapshot.GetApdexValue(options.Context, options.Name)
                        : metrics.Snapshot.GetApdexValue(options.Context, options.Name, tags);
                    return registry.PerformCheck(passing, warning, failing, value);
                });

            return registry;
        }
#pragma warning restore SA1008, SA1009
    }
}