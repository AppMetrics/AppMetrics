// <copyright file="ReportingApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Reporting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class ReportingApplicationBuilderExtensions
    {
        /// <summary>
        ///     Runs the configured App Metrics Reporting options once the application has started.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <param name="lifetime">The <see cref="T:Microsoft.AspNetCore.Hosting.IApplicationLifetime" />.</param>
        /// <returns>
        ///     A reference to this instance after the operation has completed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" /> &amp;
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IApplicationLifetime" /> cannot be null
        /// </exception>
        public static IApplicationBuilder UseMetricsReporting(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (lifetime == null)
            {
                throw new ArgumentNullException(nameof(lifetime));
            }

            var reportFactory = app.ApplicationServices.GetRequiredService<IReportFactory>();
            var metrics = app.ApplicationServices.GetRequiredService<IMetrics>();
            var reporter = reportFactory.CreateReporter();

            lifetime.ApplicationStarted.Register(() => { Task.Run(() => reporter.RunReports(metrics, lifetime.ApplicationStopping), lifetime.ApplicationStopping); });

            return app;
        }
    }
}