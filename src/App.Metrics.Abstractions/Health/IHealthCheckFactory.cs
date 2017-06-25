// <copyright file="IHealthCheckFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public interface IHealthCheckFactory
    {
        ConcurrentDictionary<string, HealthCheck> Checks { get; }

        Lazy<IMetrics> Metrics { get; }

        void Register(string name, Func<ValueTask<HealthCheckResult>> check);

        void Register(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check);
    }
}