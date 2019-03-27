// <copyright file="HostedMetricsReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using App.Metrics.Builder;
using App.Metrics.Formatters;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics;
using App.Metrics.Reporting.GrafanaCloudHostedMetrics;
using App.Metrics.Reporting.GrafanaCloudHostedMetrics.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring GrafanaCloud Hosted Metrics reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class HostedMetricsReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="HostedMetricsReporter" /> allowing metrics to be reported to HostedMetrics.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The HostedMetrics reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToHostedMetrics(
            this IMetricsReportingBuilder metricsReportingBuilder,
            MetricsReportingHostedMetricsOptions options)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new HostedMetricsReporter(options, httpClient);

            return metricsReportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="HostedMetricsReporter" /> allowing metrics to be reported to HostedMetrics.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The HostedMetrics reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToHostedMetrics(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingHostedMetricsOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingHostedMetricsOptions();

            setupAction?.Invoke(options);

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new HostedMetricsReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="HostedMetricsReporter" /> allowing metrics to be reported to HostedMetrics.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where metrics are written.</param>
        /// <param name="apiKey">The api key used for authentication</param>
        /// <param name="fieldsSetup">The metric fields to report as well as their names.</param>
        /// <param name="hostedMetricsOptionsSetup">The setup action to configure the <see cref="MetricsHostedMetricsOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToHostedMetrics(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string apiKey,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsHostedMetricsOptions> hostedMetricsOptionsSetup = null)
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

            var hostedMetricsOptions = new MetricsHostedMetricsOptions();

            var options = new MetricsReportingHostedMetricsOptions
                          {
                              HostedMetrics =
                              {
                                  BaseUri = uri,
                                  ApiKey = apiKey
                              }
                          };

            hostedMetricsOptionsSetup?.Invoke(hostedMetricsOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsHostedMetricsJsonOutputFormatter(options.FlushInterval, hostedMetricsOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsHostedMetricsJsonOutputFormatter(options.FlushInterval, hostedMetricsOptions, fields);
            }

            options.MetricsOutputFormatter = formatter;

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new HostedMetricsReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsGrafanaCloudHostedMetricsGraphiteSyntax(hostedMetricsOptions, options.FlushInterval, fields);

            return builder;
        }

        /// <summary>
        ///     Add the <see cref="HostedMetricsReporter" /> allowing metrics to be reported to HostedMetrics.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where metrics are written.</param>
        /// <param name="apiKey">The api key used for authentication</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <param name="fieldsSetup">The metric fields to report as well as their names.</param>
        /// <param name="hostedMetricsOptionsSetup">The setup action to configure the <see cref="MetricsHostedMetricsOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToHostedMetrics(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string apiKey,
            TimeSpan flushInterval,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsHostedMetricsOptions> hostedMetricsOptionsSetup = null)
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

            var hostedMetricsOptions = new MetricsHostedMetricsOptions();

            var options = new MetricsReportingHostedMetricsOptions
                          {
                              FlushInterval = flushInterval,
                              HostedMetrics =
                              {
                                  BaseUri = uri,
                                  ApiKey = apiKey
                              }
                          };

            hostedMetricsOptionsSetup?.Invoke(hostedMetricsOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsHostedMetricsJsonOutputFormatter(options.FlushInterval, hostedMetricsOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsHostedMetricsJsonOutputFormatter(options.FlushInterval, hostedMetricsOptions, fields);
            }

            options.MetricsOutputFormatter = formatter;

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new HostedMetricsReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsGrafanaCloudHostedMetricsGraphiteSyntax(hostedMetricsOptions, options.FlushInterval, fields);

            return builder;
        }

        internal static IHostedMetricsClient CreateClient(
            MetricsReportingHostedMetricsOptions options,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var httpClient = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            httpClient.BaseAddress = options.HostedMetrics.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.HostedMetrics.ApiKey);

            return new DefaultHostedMetricsHttpClient(
                httpClient,
                options.HostedMetrics,
                httpPolicy);
        }
    }
}