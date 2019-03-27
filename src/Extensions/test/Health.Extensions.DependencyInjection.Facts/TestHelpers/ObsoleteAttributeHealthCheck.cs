// <copyright file="ObsoleteAttributeHealthCheck.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace Health.Extensions.DependencyInjection.Facts.TestHelpers
{
    [Obsolete]
    public class ObsoleteAttributeHealthCheck : HealthCheck
    {
        public ObsoleteAttributeHealthCheck()
            : base("Referencing Assembly - Obsolete Check") { }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken token = default)
        {
            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("OK"));
        }
    }
}