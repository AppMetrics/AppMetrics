// <copyright file="TaskCache.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Internal
{
    internal static class TaskCache
    {
        public static readonly Task<HealthCheckResult> HealthCheckResultDegradedCompletedTask = Task.FromResult(HealthCheckResult.Degraded());
        public static readonly Task<HealthCheckResult> HealthCheckResultHealthyCompletedTask = Task.FromResult(HealthCheckResult.Healthy());
        public static readonly Task<HealthCheckResult> HealthCheckResultUnHealthyCompletedTask = Task.FromResult(HealthCheckResult.Unhealthy());
    }
}