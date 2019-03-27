// <copyright file="DefaultMetricsTrackingStartupFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.AspNetCore.Tracking
{
    /// <summary>
    /// Inserts the App Metrics Tracking at the beginning of the pipeline.
    /// </summary>
    public class DefaultMetricsTrackingStartupFilter : IStartupFilter
    {
        /// <inheritdoc />
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddMetricsTrackingMiddleware;

            void AddMetricsTrackingMiddleware(IApplicationBuilder builder)
            {
                builder.UseMetricsAllMiddleware();

                next(builder);
            }
        }
    }
}