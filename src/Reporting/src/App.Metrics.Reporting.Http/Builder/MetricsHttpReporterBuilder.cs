// <copyright file="MetricsHttpReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.Http;
using App.Metrics.Reporting.Http.Client;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics HTTP reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsHttpReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="HttpMetricsReporter" /> allowing metrics to be reported over HTTP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The HTTP reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            MetricsReportingHttpOptions options)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var provider = new HttpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="HttpMetricsReporter" /> allowing metrics to be reported over HTTP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The HTTP reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            Action<MetricsReportingHttpOptions> setupAction)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            var options = new MetricsReportingHttpOptions();

            setupAction?.Invoke(options);

            var provider = new HttpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="HttpMetricsReporter" /> allowing metrics to be reported over HTTP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="endpoint">The HTTP endpoint where metrics are POSTed.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string endpoint)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{nameof(endpoint)} must be a valid absolute URI");
            }

            var options = new MetricsReportingHttpOptions { HttpSettings = new HttpSettings(uri) };
            var provider = new HttpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }

        /// <summary>
        ///     Add the <see cref="HttpMetricsReporter" /> allowing metrics to be reported over HTTP.
        /// </summary>
        /// <param name="metricReporterProviderBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="endpoint">The HTTP endpoint where metrics are POSTed.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder OverHttp(
            this IMetricsReportingBuilder metricReporterProviderBuilder,
            string endpoint,
            TimeSpan flushInterval)
        {
            if (metricReporterProviderBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricReporterProviderBuilder));
            }

            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{nameof(endpoint)} must be a valid absolute URI");
            }

            var options = new MetricsReportingHttpOptions
                          {
                              HttpSettings = new HttpSettings(uri),
                              FlushInterval = flushInterval
                          };

            var provider = new HttpMetricsReporter(options);

            return metricReporterProviderBuilder.Using(provider);
        }
    }
}