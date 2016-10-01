using System;
using System.Collections.Generic;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class MetricsReports : IHideObjectMembers, IDisposable
    {
        private readonly Func<HealthStatus> healthStatus;
        private readonly MetricsDataProvider metricsDataProvider;

        private readonly List<ScheduledReporter> reports = new List<ScheduledReporter>();

        public MetricsReports(MetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus)
        {
            this.metricsDataProvider = metricsDataProvider;
            this.healthStatus = healthStatus;
        }

        public void Dispose()
        {
            StopAndClearAllReports();
        }

        /// <summary>
        ///     Stop all registered reports and clear the registrations.
        /// </summary>
        public void StopAndClearAllReports()
        {
            this.reports.ForEach(r => r.Dispose());
            this.reports.Clear();
        }

        /// <summary>
        ///     Schedule a Console Report to be executed and displayed on the console at a fixed <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to display the report on the Console.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public MetricsReports WithConsoleReport(TimeSpan interval, MetricsFilter filter = null)
        {
            return WithReport(new ConsoleReport(), interval, filter);
        }

        /// <summary>
        ///     Schedule a generic reporter to be executed at a fixed <paramref name="interval" />
        /// </summary>
        /// <param name="report">Function that returns an instance of a reporter</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public MetricsReports WithReport(MetricsReport report, TimeSpan interval, MetricsFilter filter = null)
        {
            var newReport = new ScheduledReporter(report, this.metricsDataProvider.WithFilter(filter), this.healthStatus, interval);
            this.reports.Add(newReport);
            return this;
        }

        /// <summary>
        ///     Schedule a Human Readable report to be executed and appended to a text file.
        /// </summary>
        /// <param name="filePath">File where to append the report.</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public MetricsReports WithTextFileReport(string filePath, TimeSpan interval, MetricsFilter filter = null)
        {
            return WithReport(new TextFileReport(filePath), interval, filter);
        }
    }
}