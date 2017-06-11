// <copyright file="MetricsHostBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    internal sealed class MetricsHostBuilder : IMetricsHostBuilder
    {
        internal MetricsHostBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = new MetricsAppEnvironment();
        }

        public IMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }
    }
}