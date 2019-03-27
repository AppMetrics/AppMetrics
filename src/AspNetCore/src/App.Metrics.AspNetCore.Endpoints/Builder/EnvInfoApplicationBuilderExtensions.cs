// <copyright file="EnvInfoApplicationBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Endpoints.Middleware;
using App.Metrics.Extensions.DependencyInjection.Internal;
using App.Metrics.Formatters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Environment Information to the request
    ///     execution pipeline.
    /// </summary>
    public static class EnvInfoApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Environment Information middleware to the <see cref="IApplicationBuilder" /> request execution
        ///     pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseEnvInfoEndpoint(this IApplicationBuilder app)
        {
            EnsureMetricsAdded(app);

            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseEnvInfoMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics Environment Information middleware to the <see cref="IApplicationBuilder" /> request execution
        ///     pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides all configured <see cref="IEnvOutputFormatter" />, matching on accept headers
        ///     won't apply.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseEnvInfoEndpoint(this IApplicationBuilder app, IEnvOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseEnvInfoMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, formatter);

            return app;
        }

        private static void EnsureMetricsAdded(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling using middleware.
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);
        }

        private static IEnvResponseWriter GetEnvInfoResponseWriter(IServiceProvider serviceProvider, IEnvOutputFormatter formatter = null)
        {
            var formatters = serviceProvider.GetRequiredService<IReadOnlyCollection<IEnvOutputFormatter>>();

            if (formatter != null)
            {
                var responseWriter = new DefaultEnvResponseWriter(formatter, formatters);
                return responseWriter;
            }

            var options = serviceProvider.GetRequiredService<IOptions<MetricEndpointsOptions>>();
            return new DefaultEnvResponseWriter(options.Value.EnvInfoEndpointOutputFormatter, formatters);
        }

        private static bool ShouldUseEnvInfo(
            IOptions<MetricsEndpointsHostingOptions> endpointHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            HttpContext context)
        {
            int? port = null;

            if (endpointHostingOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointHostingOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointHostingOptionsAccessor.Value.EnvironmentInfoEndpointPort.HasValue)
            {
                port = endpointHostingOptionsAccessor.Value.EnvironmentInfoEndpointPort.Value;
            }

            return endpointsOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                   endpointHostingOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent() &&
                   context.Request.Path == endpointHostingOptionsAccessor.Value.EnvironmentInfoEndpoint &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UseEnvInfoMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsEndpointsHostingOptions> endpointHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            IEnvOutputFormatter formatter = null)
        {
            if (endpointsOptionsAccessor.Value.EnvironmentInfoEndpointEnabled &&
                endpointHostingOptionsAccessor.Value.EnvironmentInfoEndpoint.IsPresent())
            {
                app.UseWhen(
                    context => ShouldUseEnvInfo(endpointHostingOptionsAccessor, endpointsOptionsAccessor, context),
                    appBuilder =>
                    {
                        var responseWriter = GetEnvInfoResponseWriter(appBuilder.ApplicationServices, formatter);
                        appBuilder.UseMiddleware<EnvInfoMiddleware>(responseWriter);
                    });
            }
        }
    }
}