// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Registries
{
    public interface IHealthCheckRegistry
    {
        ConcurrentDictionary<string, HealthCheck> Checks { get; }

        void Register(string name, Func<Task<string>> check);

        void Register(string name, Func<Task<HealthCheckResult>> check);

        void UnregisterAllHealthChecks();
    }
}