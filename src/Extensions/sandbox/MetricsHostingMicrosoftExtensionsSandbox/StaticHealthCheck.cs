// <copyright file="StaticHealthCheck.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MetricsHostingMicrosoftExtensionsSandbox
{
    public class StaticHealthCheck : IHealthCheck
    {
        private readonly HealthCheckResult _result;

        public StaticHealthCheck(HealthCheckResult result)
        {
            _result = result;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_result);
        }
    }
}