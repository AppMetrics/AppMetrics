// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal class NoOpHealthCheckFactory : IHealthCheckFactory
    {
        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        public void Register(string name, Func<Task<string>> check) { }

        public void Register(string name, Func<Task<HealthCheckResult>> check) { }
    }
}