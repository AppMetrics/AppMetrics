// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Extensions.Owin.Middleware;
using App.Metrics.Internal;
using App.Metrics.Serialization.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Owin;

// ReSharper disable CheckNamespace

namespace App.Metrics
// ReSharper restore CheckNamespace
{
    public static class OwinMetricsAppBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Middleware to the <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> request
        ///     execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        ///     A reference to this instance after the operation has completed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> cannot
        ///     be null
        /// </exception>
        public static IAppBuilder UseMetrics(this IAppBuilder app, IServiceProvider provider)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(provider);

            var appMetricsOptions = provider.GetRequiredService<AppMetricsOptions>();
            var owinMetricsOptions = provider.GetRequiredService<OwinMetricsOptions>();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var metrics = provider.GetRequiredService<IMetrics>();

            app.Use(new PingEndpointMiddleware(owinMetricsOptions,loggerFactory, metrics));

            if (owinMetricsOptions.HealthEndpointEnabled)
            {
                var healthSerializer = provider.GetRequiredService<IHealthStatusSerializer>();

                HealthServicesHelper.ThrowIfMetricsNotRegistered(provider);

                app.Use(new HealthCheckEndpointMiddleware(owinMetricsOptions, loggerFactory, metrics, healthSerializer));
            }

            if (owinMetricsOptions.MetricsTextEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                app.Use(new MetricsEndpointTextEndpointMiddleware(owinMetricsOptions, loggerFactory, metrics));
            }

            if (owinMetricsOptions.MetricsEndpointEnabled && appMetricsOptions.MetricsEnabled)
            {
                var metricsSerializer = provider.GetRequiredService<IMetricDataSerializer>();
                app.Use(new MetricsEndpointMiddleware(owinMetricsOptions, loggerFactory, metrics, metricsSerializer));
            }

            if (appMetricsOptions.MetricsEnabled)
            {
                app.Use(new ActiveRequestCounterEndpointMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new ErrorRequestMeterMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new OAuth2ClientWebRequestMeterMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new PerRequestTimerMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new PostAndPutRequestSizeHistogramMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new RequestTimerMiddleware(owinMetricsOptions, loggerFactory, metrics));
                app.Use(new ApdexMiddleware(owinMetricsOptions, loggerFactory, metrics));
            }

            return app;
        }

        //TODO: AH - owin configure reporting
        ///// <summary>
        /////     Runs the configured App Metrics Reporting options once the application has started.
        ///// </summary>
        ///// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        ///// <param name="lifetime">The <see cref="T:Microsoft.AspNetCore.Hosting.IApplicationLifetime" />.</param>
        ///// <returns>
        /////     A reference to this instance after the operation has completed.
        ///// </returns>
        ///// <exception cref="System.ArgumentNullException">
        /////     <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> &amp;
        /////     <see cref="T:Microsoft.AspNetCore.Hosting.IApplicationLifetime" /> cannot be null
        ///// </exception>
        //public static IAppBuilder UseMetricsReporting(this IAppBuilder app)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }

        //    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
        //    {
        //        var reportFactory = provider.GetRequiredService<IReportFactory>();
        //        var metrics = provider.GetRequiredService<IMetrics>();
        //        var reporter = reportFactory.CreateReporter();
        //        reporter.RunReports(metrics, cancellationToken);
        //    });

        //    var reportFactory = app.ApplicationServices.GetRequiredService<IReportFactory>();
        //    var metrics = app.ApplicationServices.GetRequiredService<IMetrics>();
        //    var reporter = reportFactory.CreateReporter();

        //    lifetime.ApplicationStarted.Register(() => { reporter.RunReports(metrics, lifetime.ApplicationStopping); });

        //    return app;
        //}
    }
}