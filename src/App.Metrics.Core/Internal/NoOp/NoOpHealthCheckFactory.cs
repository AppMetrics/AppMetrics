// <copyright file="NoOpHealthCheckFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core.Internal
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal sealed class NoOpHealthCheckFactory : IHealthCheckFactory
    {
        public ConcurrentDictionary<string, HealthCheck> Checks { get; } = new ConcurrentDictionary<string, HealthCheck>();

        /// <inheritdoc />
        public Lazy<IMetrics> Metrics { get; } = new Lazy<IMetrics>();

        public void Register(string name, Func<ValueTask<HealthCheckResult>> check) { }
    }
}