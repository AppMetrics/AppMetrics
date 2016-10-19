using System;
using System.Diagnostics;
using System.Threading;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IScheduledReporter
    {
        private readonly TimeSpan _interval;
        private readonly IMetricsContext _metricsContext;
        private readonly IMetricsReport _report;
        private readonly IScheduler _scheduler;
        private readonly ILogger _logger;
        private bool _disposed = false;

        public ScheduledReporter(
            ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricsReport reporter,
            TimeSpan interval)
            : this(metricsContext, reporter, interval, new ActionScheduler())
        {
            _logger = loggerFactory.CreateLogger<ScheduledReporter>();
        }

        public ScheduledReporter(
            IMetricsContext metricsContext,
            IMetricsReport report,
            TimeSpan interval,
            IScheduler scheduler)
        {
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            _metricsContext = metricsContext;
            _report = report;
            _interval = interval;
            _scheduler = scheduler;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Start(CancellationToken token)
        {
            _scheduler.Start(_interval, t => ReportAction(t));
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }

                _scheduler.Dispose();
                _report.Dispose();
            }

            _disposed = true;
        }

        private void ReportAction(CancellationToken token)
        {
            _logger.ReportRunning(_report);

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            _report.RunReport(_metricsContext, token);

            _logger.ReportRan(_report, startTimestamp);
        }
    }
}