// <copyright file="NoOpHealthCheckFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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