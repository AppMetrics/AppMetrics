// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace System.Threading.Tasks
    // ReSharper restore CheckNamespace
{
    internal static class AppMetricsTaskCache
    {
        public static readonly Task<HealthCheckResult> CompletedDegradedTask = Task.FromResult(HealthCheckResult.Degraded());
        public static readonly Task<HealthCheckResult> CompletedHealthyTask = Task.FromResult(HealthCheckResult.Healthy());

        public static readonly Task<HealthCheckResult> CompletedUnHealthyTask = Task.FromResult(HealthCheckResult.Unhealthy());

        public static readonly Task<MetricsDataValueSource> EmptyMetricsDataTask = Task.FromResult(MetricsDataValueSource.Empty);
        public static readonly Task<string> EmptyStringTask = Task.FromResult(string.Empty);
#if NET452
        public static readonly Task CompletedTask = Task.FromResult(0);
#else
        public static readonly Task CompletedTask = Task.CompletedTask;
#endif
        public static readonly Task<bool> FailedTask = Task.FromResult(false);

        public static readonly Task<bool> SuccessTask = Task.FromResult(true);
    }
}