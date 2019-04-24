// <copyright file="MetricsGraphiteReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Graphite;
using App.Metrics.Reporting.Graphite;
using App.Metrics.Reporting.Graphite.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics Grahpite reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsGraphiteReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="GraphiteReporter" /> allowing metrics to be reported to Graphite.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The Graphite reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToGraphite(
            this IMetricsReportingBuilder metricsReportingBuilder,
            MetricsReportingGraphiteOptions options)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var httpClient = CreateClient(options, options.ClientPolicy);
            var reporter = new GraphiteReporter(options, httpClient);

            return metricsReportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="GraphiteReporter" /> allowing metrics to be reported to Graphite.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The Graphite reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToGraphite(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingGraphiteOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingGraphiteOptions();

            setupAction?.Invoke(options);

            var httpClient = CreateClient(options, options.ClientPolicy);
            var reporter = new GraphiteReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="GraphiteReporter" /> allowing metrics to be reported to Graphite.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where metrics are written.</param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToGraphite(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            Action<MetricFields> fieldsSetup = null)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{nameof(url)} must be a valid absolute URI");
            }

            IMetricsOutputFormatter formatter;
            var defaultFields = new MetricFields();
            defaultFields.DefaultGraphiteMetricFieldNames();

            var plainTextProtocolOptions = new MetricsGraphitePlainTextProtocolOptions();

            if (fieldsSetup == null)
            {
                formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(plainTextProtocolOptions, defaultFields);
            }
            else
            {
                fieldsSetup.Invoke(defaultFields);
                formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(plainTextProtocolOptions, defaultFields);
            }

            var options = new MetricsReportingGraphiteOptions
                          {
                              Graphite =
                              {
                                  BaseUri = uri
                              },
                              MetricsOutputFormatter = formatter
                          };

            var httpClient = CreateClient(options, options.ClientPolicy);
            var reporter = new GraphiteReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsGraphitePlainTextProtocol(defaultFields);

            return builder;
        }

        /// <summary>
        ///     Add the <see cref="GraphiteReporter" /> allowing metrics to be reported to Graphite.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where metrics are written.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <param name="optionsSetup">The setup action to configure the <see cref="MetricsGraphitePlainTextProtocolOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToGraphite(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            TimeSpan flushInterval,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsGraphitePlainTextProtocolOptions> optionsSetup = null)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{nameof(url)} must be a valid absolute URI");
            }

            var plainTextProtocolOptions = new MetricsGraphitePlainTextProtocolOptions();

            optionsSetup?.Invoke(plainTextProtocolOptions);

            IMetricsOutputFormatter formatter;
            var defaultFields = new MetricFields();
            defaultFields.DefaultGraphiteMetricFieldNames();

            if (fieldsSetup == null)
            {
                formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(plainTextProtocolOptions, defaultFields);
            }
            else
            {
                fieldsSetup.Invoke(defaultFields);
                formatter = new MetricsGraphitePlainTextProtocolOutputFormatter(plainTextProtocolOptions, defaultFields);
            }

            var options = new MetricsReportingGraphiteOptions
                          {
                              FlushInterval = flushInterval,
                              Graphite =
                              {
                                  BaseUri = uri
                              },
                              MetricsOutputFormatter = formatter
                          };

            var httpClient = CreateClient(options, options.ClientPolicy);
            var reporter = new GraphiteReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsGraphitePlainTextProtocol(plainTextProtocolOptions, defaultFields);

            return builder;
        }

        internal static IGraphiteClient CreateClient(
            MetricsReportingGraphiteOptions options,
            ClientPolicy clientPolicy)
        {
            if (options.Graphite.Protocol == Protocol.Tcp)
            {
                return new DefaultGraphiteTcpClient(options.Graphite, clientPolicy);
            }

            if (options.Graphite.Protocol == Protocol.Udp)
            {
                return new DefaultGraphiteUdpClient(options.Graphite, clientPolicy);
            }

            if (options.Graphite.Protocol == Protocol.Pickled)
            {
                throw new NotImplementedException("Picked protocol not implemented, use UDP or TCP");
            }

            throw new NotSupportedException("Unsupported protocol, UDP, TCP and Pickled protocols are accepted");
        }
    }
}