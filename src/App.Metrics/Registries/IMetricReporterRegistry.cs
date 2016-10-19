// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Reporters;

namespace App.Metrics.Registries
{
    public interface IMetricReporterRegistry
    {
        void Dispose();

        void RunReports(CancellationToken token);

        /// <summary>
        ///     Stop all registered reports and clear the registrations.
        /// </summary>
        void StopAndClearAllReports();

        /// <summary>
        ///     Schedule a Console Report to be executed and displayed on the console at a fixed <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to display the report on the Console.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        IMetricReporterRegistry WithConsoleReport(TimeSpan interval, IMetricsFilter filter = null);

        /// <summary>
        ///     Schedule a generic reporter to be executed at a fixed <paramref name="interval" />
        /// </summary>
        /// <param name="report">Function that returns an instance of a reporter</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        IMetricReporterRegistry WithReport(IMetricsReport report, TimeSpan interval, IMetricsFilter filter = null);

        /// <summary>
        ///     Schedule a Human Readable report to be executed and appended to a text file.
        /// </summary>
        /// <param name="filePath">File where to append the report.</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        IMetricReporterRegistry WithTextFileReport(string filePath, TimeSpan interval, IMetricsFilter filter = null);
    }
}