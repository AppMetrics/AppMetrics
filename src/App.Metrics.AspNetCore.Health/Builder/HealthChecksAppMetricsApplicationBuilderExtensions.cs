// <copyright file="HealthChecksAppMetricsApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.AspNetCore.Health.Options;
using App.Metrics.Health.DependencyInjection.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class HealthChecksAppMetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health Checks Middleware to the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> cannot be null
        /// </exception>
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddHealthChecks was done before calling UseHealthChecks
            // We use the HealthCheckMarkerService to make sure if all the services were added.
            AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

            var aspNetMetricsMiddlewareHealthChecksOptions = app.ApplicationServices.GetRequiredService<AppMetricsMiddlewareHealthChecksOptions>();

            if (aspNetMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled)
            {
                HealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

                app.UseMiddleware<HealthCheckEndpointMiddleware>();
            }

            return app;
        }
    }
}