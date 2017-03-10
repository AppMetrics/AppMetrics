// <copyright file="IHealthCheckFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace App.Metrics.Health.Abstractions
{
    public interface IHealthCheckFactory
    {
        ConcurrentDictionary<string, HealthCheck> Checks { get; }

        void Register(string name, Func<Task<string>> check);

        void Register(string name, Func<Task<HealthCheckResult>> check);
    }
}