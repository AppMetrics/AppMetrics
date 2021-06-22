// <copyright file="MetricsInfluxDbReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using App.Metrics.Builder;
using App.Metrics.Formatters;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Reporting.InfluxDB;
using App.Metrics.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.InfluxDB2;
using App.Metrics.Reporting.InfluxDB2.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics InfluxDB reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsInfluxDb2ReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="InfluxDbMetricsReporter" /> allowing metrics to be reported to InfluxDB.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The InfluxDB reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb2(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            MetricsReportingInfluxDb2Options options)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var httpClient = CreateClient(options.InfluxDb2, options.HttpPolicy);
            var reporter = new InfluxDb2MetricsReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="InfluxDbMetricsReporter" /> allowing metrics to be reported to InfluxDB.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The InfluxDB reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb2(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingInfluxDb2Options> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingInfluxDb2Options();

            setupAction?.Invoke(options);

            var httpClient = CreateClient(options.InfluxDb2, options.HttpPolicy);
            var reporter = new InfluxDb2MetricsReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="InfluxDbMetricsReporter" /> allowing metrics to be reported to InfluxDB.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where InfluxDB is hosted.</param>
        /// <param name="organization">The InfluxDB organization where metrics should be flushed.</param>
        /// <param name="bucket">The InfluxDB bucket where metrics should be flushed.</param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <param name="lineProtocolOptionsSetup">The setup action to configure the <see cref="MetricsInfluxDbLineProtocolOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb2(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string organization,
            string bucket,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsInfluxDbLineProtocolOptions> lineProtocolOptionsSetup = null)
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

            var lineProtocolOptions = new MetricsInfluxDbLineProtocolOptions();

            lineProtocolOptionsSetup?.Invoke(lineProtocolOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsInfluxDbLineProtocolOutputFormatter(lineProtocolOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsInfluxDbLineProtocolOutputFormatter(lineProtocolOptions, fields);
            }

            var options = new MetricsReportingInfluxDb2Options
                          {
                              InfluxDb2 =
                              {
                                  BaseUri = uri,
                                  Organization = organization,
                                  Bucket = bucket
                              },
                MetricsOutputFormatter = formatter
            };

            var httpClient = CreateClient(options.InfluxDb2, options.HttpPolicy);
            var reporter = new InfluxDb2MetricsReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);
            builder.OutputMetrics.AsInfluxDbLineProtocol(lineProtocolOptions, fields);

            return builder;
        }

        /// <summary>
        ///     Add the <see cref="InfluxDbMetricsReporter" /> allowing metrics to be reported to InfluxDB.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where InfluxDB is hosted.</param>
        /// <param name="organization">The InfluxDB organization where metrics should be flushed.</param>
        /// <param name="bucket">The InfluxDB bucket where metrics should be flushed.</param>
        /// <param name="token">Authentication token</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <param name="lineProtocolOptionsSetup">The setup action to configure the <see cref="MetricsInfluxDbLineProtocolOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb2(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string organization,
            string bucket,
            string token,
            TimeSpan flushInterval,
            Action<MetricFields> fieldsSetup = null,
            Action<MetricsInfluxDbLineProtocolOptions> lineProtocolOptionsSetup = null)
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

            var lineProtocolOptions = new MetricsInfluxDbLineProtocolOptions();

            lineProtocolOptionsSetup?.Invoke(lineProtocolOptions);

            IMetricsOutputFormatter formatter;
            MetricFields fields = null;

            if (fieldsSetup == null)
            {
                formatter = new MetricsInfluxDbLineProtocolOutputFormatter(lineProtocolOptions);
            }
            else
            {
                fields = new MetricFields();
                fieldsSetup.Invoke(fields);
                formatter = new MetricsInfluxDbLineProtocolOutputFormatter(lineProtocolOptions, fields);
            }

            var options = new MetricsReportingInfluxDb2Options
                          {
                              FlushInterval = flushInterval,
                              InfluxDb2 =
                              {
                                  BaseUri = uri,
                                  Organization = organization,
                                  Bucket = bucket,
                                  Token = token

                              },
                              MetricsOutputFormatter = formatter
                          };

            var httpClient = CreateClient(options.InfluxDb2, options.HttpPolicy);
            var reporter = new InfluxDb2MetricsReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);

            builder.OutputMetrics.AsInfluxDbLineProtocol(lineProtocolOptions, fields);

            return builder;
        }

        internal static InfluxDb2ProtocolClient CreateClient(
            InfluxDb2Options influxDb2Options,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var httpClient = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            httpClient.BaseAddress = influxDb2Options.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;

            if (string.IsNullOrWhiteSpace(influxDb2Options.Token))
            {
                return new InfluxDb2ProtocolClient(
                    influxDb2Options,
                    httpPolicy,
                    httpClient);
            }

            httpClient.BaseAddress = influxDb2Options.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;
            if (!string.IsNullOrWhiteSpace(influxDb2Options.Token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", influxDb2Options.Token);
            }

            return new InfluxDb2ProtocolClient(
                influxDb2Options,
                httpPolicy,
                httpClient);
        }
    }
}