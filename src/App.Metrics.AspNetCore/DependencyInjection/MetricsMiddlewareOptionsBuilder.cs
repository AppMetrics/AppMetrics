// <copyright file="MetricsMiddlewareOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    internal sealed class MetricsMiddlewareOptionsBuilder : IMetricsMiddlewareOptionsBuilder
    {
        internal MetricsMiddlewareOptionsBuilder(IMetricsHostBuilder metricsHostBuilder)
        {
            MetricsHostBuilder = metricsHostBuilder ?? throw new ArgumentNullException(nameof(metricsHostBuilder));
        }

        public IMetricsHostBuilder MetricsHostBuilder { get; }
    }
}