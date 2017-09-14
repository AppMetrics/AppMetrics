// <copyright file="IMetricsRoot.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting;

namespace App.Metrics
{
    public interface IMetricsRoot : IMetrics
    {
        /// <summary>
        ///     Gets a list of <see cref="IMetricsOutputFormatter" />s that are used by this application to format metric
        ///     results.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IMetricsOutputFormatter" />s that are used by this application.
        /// </value>
        MetricsFormatterCollection OutputMetricsFormatters { get; }

        /// <summary>
        ///     Gets the default <see cref="IMetricsOutputFormatter" /> to use when metrics are attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IMetricsOutputFormatter" />s that is used by this application.
        /// </value>
        IMetricsOutputFormatter DefaultOutputMetricsFormatter { get; }

        /// <summary>
        ///     Gets the default <see cref="IEnvOutputFormatter" /> to use when the environment's info is attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IEnvOutputFormatter" />s that is used by this application.
        /// </value>
        IEnvOutputFormatter DefaultOutputEnvFormatter { get; }

        /// <summary>
        ///     Gets a list of <see cref="IEnvOutputFormatter" />s that are used by this application to format environment info.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IEnvOutputFormatter" />s that are used by this application.
        /// </value>
        EnvFormatterCollection OutputEnvFormatters { get; }

        IRunMetricsReports Reporter { get; }

        IScheduleMetricsReporting ReportScheduler { get; }

        MetricsOptions Options { get; }

        EnvironmentInfo EnvironmentInfo { get; }
    }
}