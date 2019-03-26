// <copyright file="HealthAsMetricsOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Health.Reporting.Metrics
{
    public class HealthAsMetricsOptions
    {
        /// <summary>
        ///     Gets or sets the health status reporting interval.
        /// </summary>
        /// <remarks>
        ///     If not set reporting interval will be set to the <see cref="HealthConstants.Reporting.DefaultReportInterval" />.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting health status.
        /// </value>
        public TimeSpan ReportInterval { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
