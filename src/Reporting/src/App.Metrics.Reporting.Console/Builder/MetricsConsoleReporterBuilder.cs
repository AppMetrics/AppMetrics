// <copyright file="MetricsConsoleReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.Console;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics console reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsConsoleReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="ConsoleMetricsReporter" /> allowing metrics to be reported to console.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The console reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToConsole(
            this IMetricsReportingBuilder reportingBuilder,
            MetricsReportingConsoleOptions options)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var provider = new ConsoleMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="ConsoleMetricsReporter" /> allowing metrics to be reported to console.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The console reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToConsole(
            this IMetricsReportingBuilder reportingBuilder,
            Action<MetricsReportingConsoleOptions> setupAction)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingConsoleOptions();

            setupAction?.Invoke(options);

            var provider = new ConsoleMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="ConsoleMetricsReporter" /> allowing metrics to be reported to console.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToConsole(this IMetricsReportingBuilder reportingBuilder)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            return reportingBuilder.Using<ConsoleMetricsReporter>();
        }

        /// <summary>
        ///     Add the <see cref="ConsoleMetricsReporter" /> allowing metrics to be reported to console.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToConsole(
            this IMetricsReportingBuilder reportingBuilder,
            TimeSpan flushInterval)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            return reportingBuilder.Using<ConsoleMetricsReporter>(flushInterval);
        }
    }
}