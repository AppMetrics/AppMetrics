// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Configuration;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Internal;
using AspNet.Metrics;
using AspNet.Metrics.Configuration;
using AspNet.Metrics.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNet.Builder
// ReSharper restore CheckNamespace
{
    public static class AspNetMetricsAppBuilderExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var appMetricsOptions = app.ApplicationServices.GetRequiredService<AppMetricsOptions>();
            var aspNetMetricsOptions = app.ApplicationServices.GetRequiredService<AspNetMetricsOptions>();

            app.UseMiddleware<PingEndpointMiddleware>();

            if (aspNetMetricsOptions.HealthEndpointEnabled)
            {
                HealthServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

                app.UseMiddleware<HealthCheckEndpointMiddleware>();
            }

            if (aspNetMetricsOptions.MetricsTextEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                app.UseMiddleware<MetricsEndpointTextEndpointMiddleware>();
            }

            if (aspNetMetricsOptions.MetricsEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                app.UseMiddleware<MetricsEndpointMiddleware>();
            }

            if (appMetricsOptions.MetricsEnabled)
            {
                app.UseMiddleware<ActiveRequestCounterEndpointMiddleware>();
                app.UseMiddleware<ErrorRequestMeterMiddleware>();
                app.UseMiddleware<OAuth2ClientWebRequestMeterMiddleware>();
                app.UseMiddleware<PerRequestTimerMiddleware>();
                app.UseMiddleware<PostAndPutRequestSizeHistogramMiddleware>();
                app.UseMiddleware<RequestTimerMiddleware>();
            }

            return app;
        }
    }
}