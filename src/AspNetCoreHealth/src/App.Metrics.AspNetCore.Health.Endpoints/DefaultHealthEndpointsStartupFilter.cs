// <copyright file="DefaultHealthEndpointsStartupFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore.Health.Endpoints
{
    /// <summary>
    /// Inserts App Metrics Health Endpoints at the beginning of the pipeline.
    /// </summary>
    public class DefaultHealthEndpointsStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddHealthEndpoints;

            void AddHealthEndpoints(IApplicationBuilder builder)
            {
                builder.UseHealthEndpoint();
                builder.UsePingEndpoint();

                next(builder);
            }
        }
    }
}