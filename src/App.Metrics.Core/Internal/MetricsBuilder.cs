// <copyright file="MetricsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Internal
{
    public class MetricsBuilder : IMetricsBuilder
    {
        public MetricsBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Environment = new AppMetricsEnvironment();
        }

        public IAppMetricsEnvironment Environment { get; }

        /// <summary>
        ///     Gets the <see cref="IServiceCollection" /> where App Metrics services are configured.
        /// </summary>
        /// <value>
        ///     The <see cref="IServiceCollection" /> where App Metrics services are configure.
        /// </value>
        public IServiceCollection Services { get; }
    }
}