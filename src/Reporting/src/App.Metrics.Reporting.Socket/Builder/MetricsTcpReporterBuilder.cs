// <copyright file="MetricsTcpReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Sockets;
using App.Metrics.Builder;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Socket;
using App.Metrics.Reporting.Socket.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics TCP reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsTcpReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="SocketMetricsReporter" /> allowing metrics to be reported over TCP.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The Socket reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverTcp(
            this IMetricsReportingBuilder reportingBuilder,
            MetricsReportingSocketOptions options)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            options.SocketSettings.ProtocolType = ProtocolType.Tcp;

            var provider = new SocketMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="SocketMetricsReporter" /> allowing metrics to be reported over TCP.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The Socket reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverTcp(
            this IMetricsReportingBuilder reportingBuilder,
            Action<MetricsReportingSocketOptions> setupAction)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingSocketOptions();

            setupAction?.Invoke(options);

            options.SocketSettings.ProtocolType = ProtocolType.Tcp;

            var provider = new SocketMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="SocketMetricsReporter" /> allowing metrics to be reported over TCP.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="metricsOutputFormatter">
        ///     The <see cref="IMetricsOutputFormatter" /> used to configure metrics output formatter.
        /// </param>
        /// <param name="address">The TCP endpoint address where metrics are POSTed.</param>
        /// <param name="port">The TCP endpoint port where metrics are POSTed.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverTcp(
            this IMetricsReportingBuilder reportingBuilder,
            IMetricsOutputFormatter metricsOutputFormatter,
            string address,
            int port)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingSocketOptions
            {
                SocketSettings = new SocketSettings(ProtocolType.Tcp, address, port),
                MetricsOutputFormatter = metricsOutputFormatter
            };
            var provider = new SocketMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="SocketMetricsReporter" /> allowing metrics to be reported over TCP.
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="metricsOutputFormatter">
        ///     The <see cref="IMetricsOutputFormatter" /> used to configure metrics reporters.
        /// </param>
        /// <param name="address">The TCP endpoint address where metrics are POSTed.</param>
        /// <param name="port">The TCP endpoint port where metrics are POSTed.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverTcp(
            this IMetricsReportingBuilder reportingBuilder,
            IMetricsOutputFormatter metricsOutputFormatter,
            string address,
            int port,
            TimeSpan flushInterval)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingSocketOptions
            {
                SocketSettings = new SocketSettings(ProtocolType.Tcp, address, port),
                MetricsOutputFormatter = metricsOutputFormatter,
                FlushInterval = flushInterval
            };

            var provider = new SocketMetricsReporter(options);

            return reportingBuilder.Using(provider);
        }
    }
}
