// <copyright file="MetricsApplicationBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Endpoints.Middleware;
using App.Metrics.Extensions.DependencyInjection.Internal;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics to the request execution pipeline.
    /// </summary>
    public static class MetricsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics metrics endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <remarks>
        ///     Uses the mathcing <see cref="IMetricsOutputFormatter" /> given a requests Accept header, otherwise falls back
        ///     to MetricsOptions.DefaultOutputMetricsFormatter.
        /// </remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsEndpoint(this IApplicationBuilder app)
        {
            EnsureMetricsAdded(app);

            var metricsOptions = app.ApplicationServices.GetRequiredService<MetricsOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseMetricsMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides all configured <see cref="IMetricsOutputFormatter" />, matching on accept headers
        ///     won't apply.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsEndpoint(this IApplicationBuilder app, IMetricsOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var metricsOptions = app.ApplicationServices.GetRequiredService<MetricsOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseMetricsMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions, formatter);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics text middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <remarks>By default uses the <see cref="MetricsTextOutputFormatter" /> to format results.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsTextEndpoint(this IApplicationBuilder app)
        {
            EnsureMetricsAdded(app);

            var metricsOptions = app.ApplicationServices.GetRequiredService<MetricsOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseMetricsTextMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions);

            return app;
        }

        /// <summary>
        ///     Adds App Metrics metrics text middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="formatter">
        ///     Overrides the default use of <see cref="MetricsTextOutputFormatter" /> with the
        ///     <see cref="IMetricsOutputFormatter" /> specified.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsTextEndpoint(this IApplicationBuilder app, IMetricsOutputFormatter formatter)
        {
            EnsureMetricsAdded(app);

            var metricsOptions = app.ApplicationServices.GetRequiredService<MetricsOptions>();
            var endpointHostingOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricsEndpointsHostingOptions>>();
            var endpointsOptionsAccessor = app.ApplicationServices.GetRequiredService<IOptions<MetricEndpointsOptions>>();

            UseMetricsTextMiddleware(app, endpointHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions, formatter);

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

        private static DefaultMetricsResponseWriter GetMetricsResponseWriter(
            IServiceProvider serviceProvider,
            IMetricsOutputFormatter formatter = null)
        {
            var formatters = serviceProvider.GetRequiredService<IReadOnlyCollection<IMetricsOutputFormatter>>();

            if (formatter != null)
            {
                return new DefaultMetricsResponseWriter(formatter, formatters);
            }

            var options = serviceProvider.GetRequiredService<IOptions<MetricEndpointsOptions>>();
            return new DefaultMetricsResponseWriter(options.Value.MetricsEndpointOutputFormatter, formatters);
        }

        private static IMetricsResponseWriter GetMetricsTextResponseWriter(IServiceProvider serviceProvider, IMetricsOutputFormatter formatter = null)
        {
            var formatters = serviceProvider.GetRequiredService<IReadOnlyCollection<IMetricsOutputFormatter>>();

            if (formatter != null)
            {
                var responseWriter = new DefaultMetricsResponseWriter(formatter, formatters);
                return responseWriter;
            }

            var options = serviceProvider.GetRequiredService<IOptions<MetricEndpointsOptions>>();
            return new DefaultMetricsResponseWriter(options.Value.MetricsTextEndpointOutputFormatter, formatters);
        }

        private static bool ShouldUseMetricsEndpoint(
            IOptions<MetricsEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            MetricsOptions metricsOptions,
            HttpContext context)
        {
            int? port = null;

            if (endpointsHostingOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsHostingOptionsAccessor.Value.MetricsEndpointPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.MetricsEndpointPort.Value;
            }

            return context.Request.Path == endpointsHostingOptionsAccessor.Value.MetricsEndpoint &&
                   endpointsOptionsAccessor.Value.MetricsEndpointEnabled &&
                   metricsOptions.Enabled &&
                   endpointsHostingOptionsAccessor.Value.MetricsEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static bool ShouldUseMetricsTextEndpoint(
            IOptions<MetricsEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            MetricsOptions metricsOptions,
            HttpContext context)
        {
            int? port = null;

            if (endpointsHostingOptionsAccessor.Value.AllEndpointsPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.AllEndpointsPort.Value;
            }
            else if (endpointsHostingOptionsAccessor.Value.MetricsTextEndpointPort.HasValue)
            {
                port = endpointsHostingOptionsAccessor.Value.MetricsTextEndpointPort.Value;
            }

            return context.Request.Path == endpointsHostingOptionsAccessor.Value.MetricsTextEndpoint &&
                   endpointsOptionsAccessor.Value.MetricsTextEndpointEnabled &&
                   metricsOptions.Enabled &&
                   endpointsHostingOptionsAccessor.Value.MetricsTextEndpoint.IsPresent() &&
                   (!port.HasValue || context.Features.Get<IHttpConnectionFeature>()?.LocalPort == port.Value);
        }

        private static void UseMetricsMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            MetricsOptions metricsOptions,
            IMetricsOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.MetricsEndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseMetricsEndpoint(endpointsHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions, context),
                appBuilder =>
                {
                    var responseWriter = GetMetricsResponseWriter(app.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<MetricsEndpointMiddleware>(responseWriter);
                });
        }

        private static void UseMetricsTextMiddleware(
            IApplicationBuilder app,
            IOptions<MetricsEndpointsHostingOptions> endpointsHostingOptionsAccessor,
            IOptions<MetricEndpointsOptions> endpointsOptionsAccessor,
            MetricsOptions metricsOptions,
            IMetricsOutputFormatter formatter = null)
        {
            formatter = formatter ?? endpointsOptionsAccessor.Value.MetricsTextEndpointOutputFormatter;

            app.UseWhen(
                context => ShouldUseMetricsTextEndpoint(endpointsHostingOptionsAccessor, endpointsOptionsAccessor, metricsOptions, context),
                appBuilder =>
                {
                    var responseWriter = GetMetricsTextResponseWriter(appBuilder.ApplicationServices, formatter);
                    appBuilder.UseMiddleware<MetricsEndpointMiddleware>(responseWriter);
                });
        }
    }
}