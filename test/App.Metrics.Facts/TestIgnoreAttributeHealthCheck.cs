// <copyright file="TestIgnoreAttributeHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Facts
{
    [Obsolete]
    public class TestIgnoreAttributeHealthCheck : HealthCheck
    {
        public TestIgnoreAttributeHealthCheck()
            : base("Referencing Assembly - Sample Healthy") { }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}