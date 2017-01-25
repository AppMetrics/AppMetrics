// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    // ReSharper disable InconsistentNaming
    public class InfluxDBReporterSettings : IReporterSettings
        // ReSharper restore InconsistentNaming
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InfluxDBReporterSettings" /> class.
        /// </summary>
        public InfluxDBReporterSettings()
        {
            InfluxDbSettings = new InfluxDBSettings();
            HttpPolicy = new HttpPolicy
                         {
                             FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff,
                             BackoffPeriod = Constants.DefaultBackoffPeriod,
                             Timeout = Constants.DefaultTimeout
                         };
            ReportInterval = TimeSpan.FromSeconds(5);
            MetricNameFormatter = (metricContext, metricName) => $"{metricContext}__{metricName}".Replace(' ', '_').ToLowerInvariant();
        }

        /// <summary>
        ///     Gets or sets the HTTP policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The HTTP policy.
        /// </value>
        public HttpPolicy HttpPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the influx database settings.
        /// </summary>
        /// <value>
        ///     The influx database settings.
        /// </value>
        public InfluxDBSettings InfluxDbSettings { get; set; }

        /// <summary>
        ///     Gets or sets the metric name formatter func which takes the metric context and name and returns a formatted string
        ///     which will be reported to influx as the measurement
        /// </summary>
        /// <value>
        ///     The metric name formatter.
        /// </value>
        public Func<string, string, string> MetricNameFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the report interval for which to flush metrics to influxdb.
        /// </summary>
        /// <value>
        ///     The report interval.
        /// </value>
        public TimeSpan ReportInterval { get; set; }
    }
}