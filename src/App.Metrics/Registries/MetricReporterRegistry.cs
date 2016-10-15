using System;
using System.Collections.Generic;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Reporters;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Registries
{
    public class MetricReporterRegistry : IMetricReporterRegistry, IHideObjectMembers, IDisposable
    {
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetricsDataProvider _metricsDataProvider;

        private bool _disposed = false;

        public MetricReporterRegistry(
            ILoggerFactory loggerFactory,
            IMetricsDataProvider metricsDataProvider,
            //MetricsErrorHandler errorHandler,
            IHealthCheckDataProvider healthCheckDataProvider)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            if (metricsDataProvider == null)
            {
                throw new ArgumentNullException(nameof(metricsDataProvider));
            }

            _loggerFactory = loggerFactory;
            _metricsDataProvider = metricsDataProvider;
            _healthCheckDataProvider = healthCheckDataProvider;
        }

        public List<ScheduledReporter> Reports { get; } = new List<ScheduledReporter>();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Stop all registered reports and clear the registrations.
        /// </summary>
        public void StopAndClearAllReports()
        {
            if (Reports == null) return;

            Reports.ForEach(r =>
            {
                if (r != default(ScheduledReporter))
                {
                    r.Dispose();
                }
            });

            Reports.Clear();
        }

        /// <summary>
        ///     Schedule a Console Report to be executed and displayed on the console at a fixed <paramref name="interval" />.
        /// </summary>
        /// <param name="interval">Interval at which to display the report on the Console.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public IMetricReporterRegistry WithConsoleReport(TimeSpan interval, IMetricsFilter filter = null)
        {
            return WithReport(new ConsoleReport(_loggerFactory), interval, filter);
        }

        /// <summary>
        ///     Schedule a generic reporter to be executed at a fixed <paramref name="interval" />
        /// </summary>
        /// <param name="report">Function that returns an instance of a reporter</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public IMetricReporterRegistry WithReport(IMetricsReport report, TimeSpan interval, IMetricsFilter filter = null)
        {
            var newReport = new ScheduledReporter(report, _metricsDataProvider.WithFilter(filter), _healthCheckDataProvider, interval);
            Reports.Add(newReport);
            return this;
        }

        /// <summary>
        ///     Schedule a Human Readable report to be executed and appended to a text file.
        /// </summary>
        /// <param name="filePath">File where to append the report.</param>
        /// <param name="interval">Interval at which to run the report.</param>
        /// <param name="filter">Only report metrics that match the filter.</param>
        public IMetricReporterRegistry WithTextFileReport(string filePath, TimeSpan interval, IMetricsFilter filter = null)
        {
            return WithReport(new TextFileReport(filePath, _loggerFactory), interval, filter);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }

                StopAndClearAllReports();
            }

            _disposed = true;
        }
    }
}