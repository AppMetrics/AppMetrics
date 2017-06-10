// <copyright file="SampleHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Sandbox.HealthChecks
{
    public class SampleHealthCheck : HealthCheck
    {
        public SampleHealthCheck()
#pragma warning disable SA1003, SA1028 // Symbols must be spaced correctly
            : base("Random Health Check")
#pragma warning restore SA1003, SA1028 // Symbols must be spaced correctly
        {
        }

        /// <inheritdoc />
        protected override Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (DateTime.UtcNow.Second <= 20)
            {
                return Task.FromResult(HealthCheckResult.Degraded());
            }

            if (DateTime.UtcNow.Second >= 40)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}