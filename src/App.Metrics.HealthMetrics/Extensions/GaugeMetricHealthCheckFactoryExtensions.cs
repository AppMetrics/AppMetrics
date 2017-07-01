// <copyright file="GaugeMetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Gauge;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class GaugeMetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        public static IHealthCheckRegistry AddMetricCheck(
            this IHealthCheckRegistry registry,
            string name,
            IMetrics metrics,
            GaugeOptions options,
            Func<double, (string message, bool result)> passing,
            Func<double, (string message, bool result)> warning = null,
            Func<double, (string message, bool result)> failing = null)
        {
            return registry.AddMetricCheck(name, metrics, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckRegistry AddMetricCheck(
            this IHealthCheckRegistry registry,
            string name,
            IMetrics metrics,
            GaugeOptions options,
            MetricTags tags,
            Func<double, (string message, bool result)> passing,
            Func<double, (string message, bool result)> warning = null,
            Func<double, (string message, bool result)> failing = null)
        {
            registry.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? metrics.Snapshot.GetGaugeValue(options.Context, options.Name)
                        : metrics.Snapshot.GetGaugeValue(options.Context, options.Name, tags);
                    return registry.PerformCheck(passing, warning, failing, value);
                });

            return registry;
        }
#pragma warning restore SA1008, SA1009
    }
}