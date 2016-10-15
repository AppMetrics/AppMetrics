using System;
using System.Threading;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IDisposable
    {
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;
        private readonly TimeSpan _interval;
        private readonly IMetricsDataProvider _metricsDataProvider;
        private readonly IMetricsReport _report;
        private readonly IScheduler _scheduler;
        private bool _disposed = false;

        public ScheduledReporter(IMetricsReport reporter,
            IMetricsDataProvider metricsDataProvider,
            IHealthCheckDataProvider healthCheckDataProvider,
            TimeSpan interval)
            : this(reporter, metricsDataProvider, healthCheckDataProvider, interval, new ActionScheduler())
        {
        }

        public ScheduledReporter(IMetricsReport report,
            IMetricsDataProvider metricsDataProvider,
            IHealthCheckDataProvider healthCheckDataProvider,
            TimeSpan interval,
            IScheduler scheduler)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (metricsDataProvider == null)
            {
                throw new ArgumentNullException(nameof(metricsDataProvider));
            }

            if (healthCheckDataProvider == null)
            {
                throw new ArgumentNullException(nameof(healthCheckDataProvider));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            _report = report;
            _metricsDataProvider = metricsDataProvider;
            _healthCheckDataProvider = healthCheckDataProvider;
            _interval = interval;
            _scheduler = scheduler;
        }

        public void Dispose()
        {
            Dispose(true);
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
            _report.RunReport(_metricsDataProvider.CurrentMetricsData, _healthCheckDataProvider, token);
        }

        public void Start(CancellationToken token)
        {
            _scheduler.Start(_interval, t => ReportAction(t));
        }
    }
}