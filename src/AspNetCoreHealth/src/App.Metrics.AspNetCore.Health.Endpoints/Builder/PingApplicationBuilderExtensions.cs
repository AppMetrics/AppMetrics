// <copyright file="PingApplicationBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.AspNetCore.Health.Endpoints.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Health ping pong to the request execution
    ///     pipeline.
    /// </summary>
    public static class PingApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Health Ping middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UsePingEndpoint(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var metricsEndpointsHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<HealthEndpointsOptions>>();

            UsePingMiddleware(app, metricsEndpointsHostingOptionsAccessor, endpointsOptionsAccessor);

            return app;
        }

        private static bool ShouldUsePing(
            IOptions<HealthEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<HealthEndpointsOptions> endpointsOptionsAccessor,
            HttpContext context)
        {
            int? port = null;

            if (endpointsHostingOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsHostingOptionsAccessor.Value.PingEndpointPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.PingEndpointPort.Value;
            }

            return context.Request.Path == endpointsHostingOptionsAccessor.Value.PingEndpoint &&
                   endpointsOptionsAccessor.Value.PingEndpointEnabled &&
                   endpointsHostingOptionsAccessor.Value.PingEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UsePingMiddleware(
            IApplicationBuilder app,
            IOptions<HealthEndpointsHostingOptions> metricsEndpointsHostingOptionsAccessor,
            IOptions<HealthEndpointsOptions> endpointsOptionsAccessor)
        {
            app.UseWhen(
                context => ShouldUsePing(metricsEndpointsHostingOptionsAccessor, endpointsOptionsAccessor, context),
                appBuilder => { appBuilder.UseMiddleware<PingEndpointMiddleware>(); });
        }
    }
}