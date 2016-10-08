using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IDisposable
    {
        private readonly Func<HealthStatus> _healthStatus;
        private readonly IMetricsDataProvider _metricsDataProvider;
        private readonly IMetricsReport _report;
        private readonly IScheduler _scheduler;

        public ScheduledReporter(IMetricsReport reporter, IMetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval)
            : this(reporter, metricsDataProvider, healthStatus, interval, new ActionScheduler())
        {
        }

        public ScheduledReporter(IMetricsReport report, IMetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval,
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
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            _report = report;
            _metricsDataProvider = metricsDataProvider;
            _healthStatus = healthStatus;
            _scheduler = scheduler;
            _scheduler.Start(interval, t => RunReport(t));
        }

        public void Dispose()
        {
            using (_scheduler)
            {
            }
            using (_report as IDisposable)
            {
            }
        }

        private void RunReport(CancellationToken token)
        {
            _report.RunReport(_metricsDataProvider.CurrentMetricsData, _healthStatus, token);
        }
    }
}