// <copyright file="StatsDReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Formatting.StatsD;

namespace App.Metrics.Reporting.StatsD
{
    public static class StatsDReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to be reported to StatsD over TCP.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The StatsD reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToStatsDTcp(
            this IMetricsReportingBuilder metricsReportingBuilder,
            MetricsReportingStatsDOptions options)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var formatter = new MetricsStatsDStringOutputFormatter(options.StatsDOptions);

            return metricsReportingBuilder.OverTcp(
                opt =>
                {
                    opt.MetricsOutputFormatter = formatter;
                    opt.FlushInterval = options.FlushInterval;
                    opt.SocketSettings = options.SocketSettings;
                    opt.SocketPolicy = options.SocketPolicy;
                    opt.Filter = options.Filter;
                });
        }

        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to be reported to StatsD over TCP.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The StatsD reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToStatsDTcp(
            this IMetricsReportingBuilder metricsReportingBuilder,
            Action<MetricsReportingStatsDOptions> setupAction)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var options = new MetricsReportingStatsDOptions();
            setupAction?.Invoke(options);

            var formatter = new MetricsStatsDStringOutputFormatter(options.StatsDOptions);

            return metricsReportingBuilder.OverTcp(
                opt =>
                {
                    opt.MetricsOutputFormatter = formatter;
                    opt.FlushInterval = options.FlushInterval;
                    opt.SocketSettings = options.SocketSettings;
                    opt.SocketPolicy = options.SocketPolicy;
                    opt.Filter = options.Filter;
                });
        }

        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to be reported to StatsD over UDP.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The StatsD reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToStatsDUdp(
            this IMetricsReportingBuilder metricsReportingBuilder,
            MetricsReportingStatsDOptions options)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var formatter = new MetricsStatsDStringOutputFormatter(options.StatsDOptions);

            return metricsReportingBuilder.OverUdp(
                opt =>
                {
                    opt.MetricsOutputFormatter = formatter;
                    opt.FlushInterval = options.FlushInterval;
                    opt.SocketSettings = options.SocketSettings;
                    opt.SocketPolicy = options.SocketPolicy;
                    opt.Filter = options.Filter;
                });
        }

        /// <summary>
        ///     Add the <see cref="MetricsStatsDStringOutputFormatter" /> allowing metrics to be reported to StatsD over UDP.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The StatsD reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToStatsDUdp(
            this IMetricsReportingBuilder metricsReportingBuilder,
            Action<MetricsReportingStatsDOptions> setupAction)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var options = new MetricsReportingStatsDOptions();
            setupAction?.Invoke(options);

            var formatter = new MetricsStatsDStringOutputFormatter(options.StatsDOptions);

            return metricsReportingBuilder.OverUdp(
                opt =>
                {
                    opt.MetricsOutputFormatter = formatter;
                    opt.FlushInterval = options.FlushInterval;
                    opt.SocketSettings = options.SocketSettings;
                    opt.SocketPolicy = options.SocketPolicy;
                    opt.Filter = options.Filter;
                });
        }
    }
}