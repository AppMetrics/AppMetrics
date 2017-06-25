// <copyright file="TestIgnoreAttributeHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Facts.TestHelpers
{
    [Obsolete]
    public class TestIgnoreAttributeHealthCheck : HealthCheck
    {
        public TestIgnoreAttributeHealthCheck()
            : base("Referencing Assembly - Sample Healthy") { }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("OK"));
        }
    }
}