// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Metrics.Core.Internal;
using App.Metrics.Health.Abstractions;

namespace App.Metrics.Health.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal sealed class NoOpHealthCheckFactory : IHealthCheckFactory
    {
        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        public void Register(string name, Func<Task<string>> check) { }

        public void Register(string name, Func<Task<HealthCheckResult>> check) { }
    }
}