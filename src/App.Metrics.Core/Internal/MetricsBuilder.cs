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
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }
    }
}