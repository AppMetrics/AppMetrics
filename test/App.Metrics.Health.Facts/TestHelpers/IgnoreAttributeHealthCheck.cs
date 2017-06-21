// <copyright file="IgnoreAttributeHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Facts.TestHelpers
{
    [Obsolete]
    public class IgnoreAttributeHealthCheck : HealthCheck
    {
        public IgnoreAttributeHealthCheck()
            : base("Referencing Assembly - Sample Healthy") { }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            return Task.FromResult(HealthCheckResult.Healthy("OK"));
        }
    }
}