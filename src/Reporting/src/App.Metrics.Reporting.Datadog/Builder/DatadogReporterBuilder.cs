// <copyright file="DatadogReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using App.Metrics.Builder;
using App.Metrics.Formatters;
using App.Metrics.Formatting.Datadog;
using App.Metrics.Reporting.Datadog;
using App.Metrics.Reporting.Datadog.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring Datadog reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class DatadogReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="DatadogReporter" /> allowing metrics to be reported to Datadog.
        /// </summary>
        /// <param name="metricsReportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The Datadog reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToDatadogHttp(
            this IMetricsReportingBuilder metricsReportingBuilder,
            MetricsReportingDatadogOptions options)
        {
            if (metricsReportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricsReportingBuilder));
            }

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new DatadogReporter(options, httpClient);

            return metricsReportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="DatadogReporter" /> allowing metrics to be reported to Datadog.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The Datadog reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToDatadogHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingDatadogOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingDatadogOptions();

            setupAction?.Invoke(options);

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new DatadogReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="DatadogReporter" /> allowing metrics to be reported to Datadog.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where metrics are written.</param>
        /// <param name="apiKey">The api key used for authentication</param>
        /// <param name="fieldsSetup">The metric fields to report as well as their names.</param>
        /// <param name="datadogOptionsSetup">The setup action to configure the <see cref="MetricsDatadogOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToDatadogHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string apiKey,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsDatadogOptions> datadogOptionsSetup = null)
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

            var datadogOptions = new MetricsDatadogOptions();

            var options = new MetricsReportingDatadogOptions
                          {
                              Datadog =
                              {
                                  BaseUri = uri,
                                  ApiKey = apiKey
                              }
                          };

            datadogOptionsSetup?.Invoke(datadogOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsDatadogJsonOutputFormatter(options.FlushInterval, datadogOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsDatadogJsonOutputFormatter(options.FlushInterval, datadogOptions, fields);
            }

            options.MetricsOutputFormatter = formatter;

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new DatadogReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsDatadogJson(datadogOptions, options.FlushInterval, fields);

            return builder;
        }

        /// <summary>
        ///     Add the <see cref="DatadogReporter" /> allowing metrics to be reported to Datadog.
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
        /// <param name="datadogOptionsSetup">The setup action to configure the <see cref="MetricsDatadogOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToDatadogHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string apiKey,
            TimeSpan flushInterval,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsDatadogOptions> datadogOptionsSetup = null)
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

            var datadogOptions = new MetricsDatadogOptions();

            var options = new MetricsReportingDatadogOptions
                          {
                              FlushInterval = flushInterval,
                              Datadog =
                              {
                                  BaseUri = uri,
                                  ApiKey = apiKey
                              }
                          };

            datadogOptionsSetup?.Invoke(datadogOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsDatadogJsonOutputFormatter(options.FlushInterval, datadogOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsDatadogJsonOutputFormatter(options.FlushInterval, datadogOptions, fields);
            }

            options.MetricsOutputFormatter = formatter;

            var httpClient = CreateClient(options, options.HttpPolicy);
            var reporter = new DatadogReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsDatadogJson(datadogOptions, options.FlushInterval, fields);

            return builder;
        }

        public static IDatadogClient CreateClient(
            MetricsReportingDatadogOptions options,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var httpClient = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            httpClient.BaseAddress = options.Datadog.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;

            return new DefaultDatadogHttpClient(
                httpClient,
                options.Datadog,
                httpPolicy);
        }
    }
}