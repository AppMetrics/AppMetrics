// <copyright file="IMetricsMiddlewareOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Extensions.Middleware.DependencyInjection
{
    /// <summary>
    ///     Provides extensiblity on App Metrics middleware e.g. serialization options on supported endpoints.
    /// </summary>
    public interface IMetricsMiddlewareOptionsBuilder
    {
        IMetricsHostBuilder MetricsHostBuilder { get; }
    }
}