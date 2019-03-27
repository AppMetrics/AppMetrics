// <copyright file="MetricsReportingHttpOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Http.Client;

namespace App.Metrics.Reporting.Http
{
    /// <summary>
    ///     Provides programmatic configuration of HTTP Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingHttpOptions
    {
        public MetricsReportingHttpOptions()
        {
            HttpSettings = new HttpSettings();
            HttpPolicy = new HttpPolicy();
        }

        /// <summary>
        ///     Gets or sets the HTTP policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The HTTP policy.
        /// </value>
        public HttpPolicy HttpPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP client related settings.
        /// </summary>
        /// <value>
        ///     The HTTP client settings.
        /// </value>
        public HttpSettings HttpSettings { get; set; }

        /// <summary>
        ///     Gets or sets the inner HTTP message handler to be used with the HTTP Client.
        /// </summary>
        /// <value>
        ///     The inner HTTP message handler.
        /// </value>
        public HttpMessageHandler InnerHttpMessageHandler { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the interval between flushing metrics.
        /// </summary>
        public TimeSpan FlushInterval { get; set; }
    }
}