// <copyright file="AppMetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Middleware;
using App.Metrics.AspNetCore.Middleware.Options;
using App.Metrics.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Middleware to the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> cannot be null
        /// </exception>
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            Core.DependencyInjection.Internal.AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var appMetricsOptions = app.ApplicationServices.GetRequiredService<AppMetricsOptions>();
            var appMetricsMiddlewareOptions = app.ApplicationServices.GetRequiredService<AppMetricsMiddlewareOptions>();

            if (appMetricsMiddlewareOptions.PingEndpointEnabled)
            {
                app.UseMiddleware<PingEndpointMiddleware>();
            }

            if (appMetricsMiddlewareOptions.MetricsTextEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                app.UseMiddleware<MetricsEndpointTextEndpointMiddleware>();
            }

            if (appMetricsMiddlewareOptions.MetricsEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                app.UseMiddleware<MetricsEndpointMiddleware>();
            }

            if (appMetricsMiddlewareOptions.EnvironmentInfoEndpointEnabled)
            {
                app.UseMiddleware<EnvironmentInfoMiddleware>();
            }

            if (appMetricsOptions.MetricsEnabled && appMetricsMiddlewareOptions.DefaultTrackingEnabled)
            {
                app.UseMiddleware<ActiveRequestCounterEndpointMiddleware>();
                app.UseMiddleware<ErrorRequestMeterMiddleware>();
                app.UseMiddleware<PerRequestTimerMiddleware>();
                app.UseMiddleware<OAuthTrackingMiddleware>();
                app.UseMiddleware<PostAndPutRequestSizeHistogramMiddleware>();
                app.UseMiddleware<RequestTimerMiddleware>();
            }

            if (appMetricsOptions.MetricsEnabled && appMetricsMiddlewareOptions.ApdexTrackingEnabled)
            {
                app.UseMiddleware<ApdexMiddleware>();
            }

            return app;
        }
    }
}