// <copyright file="ApdexMetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class ApdexMetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            ApdexOptions options,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            ApdexOptions options,
            MetricTags tags,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetApdexValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetApdexValue(options.Context, options.Name, tags);
                    return factory.PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }
#pragma warning restore SA1008, SA1009
    }
}