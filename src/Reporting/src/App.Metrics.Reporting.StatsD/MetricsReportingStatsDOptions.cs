// <copyright file="MetricsReportingStatsDOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Filters;
using App.Metrics.Formatting.StatsD;
using App.Metrics.Reporting.Socket.Client;
using System;

namespace App.Metrics.Reporting.StatsD
{
    public class MetricsReportingStatsDOptions
    {
        public MetricsReportingStatsDOptions()
        {
            SocketSettings = new SocketSettings();
            SocketPolicy = new SocketPolicy();
            StatsDOptions = new MetricsStatsDOptions();
        }

        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the Socket policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The Socket policy.
        /// </value>
        public SocketPolicy SocketPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the interval between flushing metrics.
        /// </summary>
        public TimeSpan FlushInterval { get; set; }

        /// <summary>
        ///     Gets or sets the Socket client related settings.
        /// </summary>
        /// <value>
        ///     The Socket client settings.
        /// </value>
        public SocketSettings SocketSettings { get; set; }

        public MetricsStatsDOptions StatsDOptions { get; set; }
    }
}