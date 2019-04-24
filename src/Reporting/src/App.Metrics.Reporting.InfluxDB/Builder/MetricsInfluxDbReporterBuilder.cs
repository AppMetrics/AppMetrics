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

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics InfluxDB reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsInfluxDbReporterBuilder
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
        public static IMetricsBuilder ToInfluxDb(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            MetricsReportingInfluxDbOptions options)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var httpClient = CreateClient(options.InfluxDb, options.HttpPolicy);
            var reporter = new InfluxDbMetricsReporter(options, httpClient);

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
        public static IMetricsBuilder ToInfluxDb(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingInfluxDbOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingInfluxDbOptions();

            setupAction?.Invoke(options);

            var httpClient = CreateClient(options.InfluxDb, options.HttpPolicy);
            var reporter = new InfluxDbMetricsReporter(options, httpClient);

            return metricReporterProviderBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="InfluxDbMetricsReporter" /> allowing metrics to be reported to InfluxDB.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="url">The base url where InfluxDB is hosted.</param>
        /// <param name="database">The InfluxDB where metrics should be flushed.</param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <param name="lineProtocolOptionsSetup">The setup action to configure the <see cref="MetricsInfluxDbLineProtocolOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string database,
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

            var options = new MetricsReportingInfluxDbOptions
                          {
                              InfluxDb =
                              {
                                  BaseUri = uri,
                                  Database = database
                              },
                              MetricsOutputFormatter = formatter
            };

            var httpClient = CreateClient(options.InfluxDb, options.HttpPolicy);
            var reporter = new InfluxDbMetricsReporter(options, httpClient);

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
        /// <param name="database">The InfluxDB where metrics should be flushed.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <param name="fieldsSetup">The metric fields to report as well as thier names.</param>
        /// <param name="lineProtocolOptionsSetup">The setup action to configure the <see cref="MetricsInfluxDbLineProtocolOptions"/> to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToInfluxDb(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string url,
            string database,
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

            var options = new MetricsReportingInfluxDbOptions
                          {
                              FlushInterval = flushInterval,
                              InfluxDb =
                              {
                                  BaseUri = uri,
                                  Database = database
                              },
                              MetricsOutputFormatter = formatter
                          };

            var httpClient = CreateClient(options.InfluxDb, options.HttpPolicy);
            var reporter = new InfluxDbMetricsReporter(options, httpClient);

            var builder = metricReporterProviderBuilder.Using(reporter);

            builder.OutputMetrics.AsInfluxDbLineProtocol(lineProtocolOptions, fields);

            return builder;
        }

        internal static ILineProtocolClient CreateClient(
            InfluxDbOptions influxDbOptions,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var httpClient = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            httpClient.BaseAddress = influxDbOptions.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;

            if (string.IsNullOrWhiteSpace(influxDbOptions.UserName) || string.IsNullOrWhiteSpace(influxDbOptions.Password))
            {
                return new DefaultLineProtocolClient(
                    influxDbOptions,
                    httpPolicy,
                    httpClient);
            }

            var byteArray = Encoding.ASCII.GetBytes($"{influxDbOptions.UserName}:{influxDbOptions.Password}");
            httpClient.BaseAddress = influxDbOptions.BaseUri;
            httpClient.Timeout = httpPolicy.Timeout;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return new DefaultLineProtocolClient(
                influxDbOptions,
                httpPolicy,
                httpClient);
        }
    }
}