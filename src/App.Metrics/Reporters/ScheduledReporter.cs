using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IDisposable
    {
        private readonly Func<HealthStatus> _healthStatus;
        private readonly MetricsDataProvider _metricsDataProvider;
        private readonly MetricsReport _report;
        private readonly Scheduler _scheduler;

        public ScheduledReporter(MetricsReport reporter, MetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval)
            : this(reporter, metricsDataProvider, healthStatus, interval, new ActionScheduler())
        {
        }

        public ScheduledReporter(MetricsReport report, MetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval,
            Scheduler scheduler)
        {
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