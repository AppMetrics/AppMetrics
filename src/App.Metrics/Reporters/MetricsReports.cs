using System;
using System.Collections.Generic;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class MetricsReports : IHideObjectMembers, IDisposable
    {
        private readonly Func<HealthStatus> _healthStatus;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetricsDataProvider _metricsDataProvider;
        private readonly MetricsErrorHandler _errorHandler;

        private readonly List<ScheduledReporter> _reports = new List<ScheduledReporter>();

        public MetricsReports(
            ILoggerFactory loggerFactory,
            IMetricsDataProvider metricsDataProvider, 
            MetricsErrorHandler errorHandler,
            Func<HealthStatus> healthStatus)
        {
            if (loggerFactory == null)
            {

                throw new ArgumentNullException(nameof(loggerFactory));
            }
            if (metricsDataProvider == null)
            {
                throw new ArgumentNullException(nameof(metricsDataProvider));
            }
            if (errorHandler == null)
            {
                throw new ArgumentNullException(nameof(errorHandler));
            }

            _loggerFactory = loggerFactory;
            _metricsDataProvider = metricsDataProvider;
            _errorHandler = errorHandler;
            _healthStatus = healthStatus;
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
            _reports.ForEach(r => r.Dispose());
            _reports.Clear();
        }

        /// <summary>
        ///     Schedule a Console Report to be executed and displayed on the console at a fixed <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to display the report on the Console.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public MetricsReports WithConsoleReport(TimeSpan interval, MetricsFilter filter = null)
        {
            return WithReport(new ConsoleReport(_loggerFactory, _errorHandler), interval, filter);
        }

        /// <summary>
        ///     Schedule a generic reporter to be executed at a fixed <paramref name="interval" />
        /// </summary>
        /// <param name="report">Function that returns an instance of a reporter</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public MetricsReports WithReport(IMetricsReport report, TimeSpan interval, MetricsFilter filter = null)
        {
            var newReport = new ScheduledReporter(report, _metricsDataProvider.WithFilter(filter), _healthStatus, interval);
            _reports.Add(newReport);
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
            return WithReport(new TextFileReport(filePath, _loggerFactory, _errorHandler), interval, filter);
        }
    }
}