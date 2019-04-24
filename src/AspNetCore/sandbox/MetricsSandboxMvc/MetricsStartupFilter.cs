// <copyright file="MetricsStartupFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace MetricsSandboxMvc
{
    /// <summary>
    /// Inserts the App Metrics Middleware at the beginning of the pipeline.
    /// </summary>
    public class MetricsStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddAllMetricsEndpointsAndTrackingMiddleware;

            void AddAllMetricsEndpointsAndTrackingMiddleware(IApplicationBuilder builder)
            {
                builder.UseMetricsEndpoint();
                builder.UseMetricsTextEndpoint();
                builder.UseEnvInfoEndpoint();
                builder.UseMetricsAllMiddleware();

                next(builder);
            }
        }
    }
}