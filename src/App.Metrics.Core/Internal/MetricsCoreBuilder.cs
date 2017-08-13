// <copyright file="MetricsCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Internal
{
    public class MetricsCoreBuilder : IMetricsCoreBuilder
    {
        public MetricsCoreBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = new AppMetricsEnvironment();
        }

        public IAppMetricsEnvironment Environment { get; }

        /// <summary>
        ///     Gets the <see cref="IServiceCollection" /> where essential App Metrics services are configured.
        /// </summary>
        /// <value>
        ///     The <see cref="IServiceCollection" /> where essential App Metrics services are configure.
        /// </value>
        public IServiceCollection Services { get; }
    }
}