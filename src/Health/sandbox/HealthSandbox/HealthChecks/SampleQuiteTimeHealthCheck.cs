// <copyright file="SampleQuiteTimeHealthCheck.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace HealthSandbox.HealthChecks
{
    public class SampleQuiteTimeHealthCheck : HealthCheck
    {
        private static readonly QuiteTime QuiteAt = new QuiteTime(new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0), new[] { DayOfWeek.Monday });

        public SampleQuiteTimeHealthCheck()
            : base("Random Health Check - Quite Time", quiteTime: QuiteAt)
        {
        }

        /// <inheritdoc />
        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default)
        {
            if (DateTime.UtcNow.Second <= 20)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded());
            }

            if (DateTime.UtcNow.Second >= 40)
            {
                return new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy());
            }

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }
    }
}