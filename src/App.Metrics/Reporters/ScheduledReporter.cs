using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IDisposable
    {
        private readonly TimeSpan _interval;
        private readonly IMetricsContext _metricsContext;
        private readonly IMetricsReport _report;
        private readonly IScheduler _scheduler;
        private bool _disposed = false;

        public ScheduledReporter(
            IMetricsContext metricsContext,
            IMetricsReport reporter,
            TimeSpan interval)
            : this(metricsContext, reporter, interval, new ActionScheduler())
        {
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
            _report.RunReport(_metricsContext, token);
        }
    }
}