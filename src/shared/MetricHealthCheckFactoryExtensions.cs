// <copyright file="MetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        internal static Task<HealthCheckResult> PerformCheck<T>(
            this IHealthCheckFactory factory,
            Func<T, (string message, bool result)> passing,
            Func<T, (string message, bool result)> warning,
            Func<T, (string message, bool result)> failing,
            T value)
        {
            if (value == null)
            {
                return Task.FromResult(HealthCheckResult.Ignore("Metric not found"));
            }

            var passingResult = passing(value);

            if (passingResult.result)
            {
                return Task.FromResult(HealthCheckResult.Healthy(passingResult.message));
            }

            if (warning != null)
            {
                var warningResult = warning(value);

                if (warningResult.result)
                {
                    return Task.FromResult(HealthCheckResult.Degraded(warningResult.message));
                }
            }

            if (failing != null)
            {
                var failingResult = failing(value);

                if (failingResult.result)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy(failingResult.message));
                }
            }

            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
#pragma warning restore SA1008, SA1009
}