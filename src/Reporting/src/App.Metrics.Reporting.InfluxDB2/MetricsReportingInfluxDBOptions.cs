// <copyright file="MetricsReportingInfluxDBOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting.InfluxDB.Client;
using App.Metrics.Reporting.InfluxDB2.Client;

namespace App.Metrics.Reporting.InfluxDB2
{
    /// <summary>
    ///     Provides programmatic configuration for InfluxDB Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingInfluxDb2Options
    {
        public MetricsReportingInfluxDb2Options()
        {
            FlushInterval = TimeSpan.FromSeconds(10);
            HttpPolicy = new HttpPolicy
                         {
                             FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff,
                             BackoffPeriod = Constants.DefaultBackoffPeriod,
                             Timeout = Constants.DefaultTimeout
                         };
            InfluxDb2 = new InfluxDb2Options();
        }

        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The HTTP policy.
        /// </value>
        public HttpPolicy HttpPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the available options for InfluxDB connectivity.
        /// </summary>
        /// <value>
        ///     The <see cref="InfluxDb2Options" />.
        /// </value>
        public InfluxDb2Options InfluxDb2 { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the flush metrics interval
        /// </summary>
        /// <remarks>
        ///     This <see cref="TimeSpan" /> will apply to all configured reporters unless overriden by a specific reporters
        ///     options.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting metrics
        /// </value>
        public TimeSpan FlushInterval { get; set; }
    }
}