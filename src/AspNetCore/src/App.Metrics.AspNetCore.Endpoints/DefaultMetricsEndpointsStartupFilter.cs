// <copyright file="DefaultMetricsEndpointsStartupFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore.Endpoints
{
    /// <summary>
    /// Inserts all App Metrics Endpoints at the beginning of the pipeline.
    /// </summary>
    public class DefaultMetricsEndpointsStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddMetricsEndpoints;

            void AddMetricsEndpoints(IApplicationBuilder builder)
            {
                builder.UseMetricsEndpoint();
                builder.UseMetricsTextEndpoint();
                builder.UseEnvInfoEndpoint();

                next(builder);
            }
        }
    }
}