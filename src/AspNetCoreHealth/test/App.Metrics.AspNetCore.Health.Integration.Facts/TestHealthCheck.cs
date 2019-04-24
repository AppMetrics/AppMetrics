// <copyright file="TestHealthCheck.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.AspNetCore.Health.Integration.Facts
{
    // ReSharper disable UnusedMember.Global this is automatically registered
    public class TestHealthCheck : HealthCheck
        // ReSharper restore UnusedMember.Global
    {
        public TestHealthCheck()
            : base("Test Health Check") { }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken token = default)
        {
            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("OK"));
        }
    }
}