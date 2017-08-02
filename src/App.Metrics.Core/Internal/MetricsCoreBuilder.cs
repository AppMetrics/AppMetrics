// <copyright file="MetricsCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Internal
{
    internal sealed class MetricsCoreBuilder : IMetricsCoreBuilder
    {
        internal MetricsCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = new AppMetricsEnvironment();
        }

        public IAppMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }
    }
}