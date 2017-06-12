// <copyright file="MeterMetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health;
using App.Metrics.Meter;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MeterMetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            MeterOptions options,
            Func<MeterValue, (string message, bool result)> passing,
            Func<MeterValue, (string message, bool result)> warning = null,
            Func<MeterValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            MeterOptions options,
            MetricTags tags,
            Func<MeterValue, (string message, bool result)> passing,
            Func<MeterValue, (string message, bool result)> warning = null,
            Func<MeterValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetMeterValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetMeterValue(options.Context, options.Name, tags);
                    return factory.PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }
#pragma warning restore SA1008, SA1009
    }
}