// <copyright file="MetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        internal static ValueTask<HealthCheckResult> PerformCheck<T>(
            this IHealthCheckRegistry registry,
            Func<T, (string message, bool result)> passing,
            Func<T, (string message, bool result)> warning,
            Func<T, (string message, bool result)> failing,
            T value)
        {
            if (value == null)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Ignore("Metric not found"));
            }

            var passingResult = passing(value);

            if (passingResult.result)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(passingResult.message));
            }

            if (warning != null)
            {
                var warningResult = warning(value);

                if (warningResult.result)
                {
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(warningResult.message));
                }
            }

            if (failing != null)
            {
                var failingResult = failing(value);

                if (failingResult.result)
                {
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(failingResult.message));
                }
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy());
        }
    }
#pragma warning restore SA1008, SA1009
}