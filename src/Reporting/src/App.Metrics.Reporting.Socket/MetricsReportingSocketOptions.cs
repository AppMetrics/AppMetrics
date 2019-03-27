// <copyright file="MetricsReportingSocketOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Socket.Client;

namespace App.Metrics.Reporting.Socket
{
    /// <summary>
    ///     Provides programmatic configuration of Socket Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingSocketOptions
    {
        public MetricsReportingSocketOptions()
        {
            SocketSettings = new SocketSettings();
            SocketPolicy = new SocketPolicy();
        }

        /// <summary>
        ///     Gets or sets the Socket policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The Socket policy.
        /// </value>
        public SocketPolicy SocketPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the Socket client related settings.
        /// </summary>
        /// <value>
        ///     The Socket client settings.
        /// </value>
        public SocketSettings SocketSettings { get; set; }

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
