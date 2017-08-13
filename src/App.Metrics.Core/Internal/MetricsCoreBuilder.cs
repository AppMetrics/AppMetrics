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
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }
    }
}