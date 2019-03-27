// <copyright file="MetricsReportingGraphiteOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Graphite.Client;

namespace App.Metrics.Reporting.Graphite
{
    public class MetricsReportingGraphiteOptions
    {
        public MetricsReportingGraphiteOptions()
        {
            FlushInterval = TimeSpan.FromSeconds(10);
            ClientPolicy = new ClientPolicy
                         {
                             FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff,
                             BackoffPeriod = Constants.DefaultBackoffPeriod,
                             Timeout = Constants.DefaultTimeout
                         };
            Graphite = new GraphiteOptions();
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
        public ClientPolicy ClientPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the available options for GraphiteOptions connectivity.
        /// </summary>
        /// <value>
        ///     The <see cref="GraphiteOptions" />.
        /// </value>
        public GraphiteOptions Graphite { get; set; }

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