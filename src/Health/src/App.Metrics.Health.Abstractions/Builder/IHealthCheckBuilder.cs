// <copyright file="IHealthCheckBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public interface IHealthCheckBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IHealthBuilder" /> where App Metrics Health is configured.
        /// </summary>
        IHealthBuilder Builder { get; }

        IHealthBuilder AddCheck(string name, Func<ValueTask<HealthCheckResult>> check);

        IHealthBuilder AddCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check);

        IHealthBuilder AddCheck<THealthCheck>(THealthCheck check)
            where THealthCheck : HealthCheck;

        IHealthBuilder AddCheck<THealthCheck>()
            where THealthCheck : HealthCheck, new();

        IHealthBuilder AddChecks(IEnumerable<HealthCheck> checks);
    }
}