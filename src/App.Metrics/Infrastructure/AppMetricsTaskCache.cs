// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using App.Metrics.MetricData;

namespace App.Metrics.Infrastructure
{
    public static class AppMetricsTaskCache
    {
        public static readonly Task EmptyTask = Task.FromResult(0);

        public static readonly Task<HealthCheckResult> CompletedHealthyTask = Task.FromResult(HealthCheckResult.Healthy());

        public static readonly Task<HealthCheckResult> CompletedUnHealthyTask = Task.FromResult(HealthCheckResult.Unhealthy());

        public static readonly Task<MetricsData> EmptyMetricsDataTask = Task.FromResult(MetricsData.Empty);
    }
}