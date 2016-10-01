using System;
using System.Threading;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporters
{
    public sealed class ScheduledReporter : IDisposable
    {
        private readonly Func<HealthStatus> healthStatus;
        private readonly MetricsDataProvider metricsDataProvider;
        private readonly MetricsReport report;
        private readonly Scheduler scheduler;

        public ScheduledReporter(MetricsReport reporter, MetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval)
            : this(reporter, metricsDataProvider, healthStatus, interval, new ActionScheduler())
        {
        }

        public ScheduledReporter(MetricsReport report, MetricsDataProvider metricsDataProvider, Func<HealthStatus> healthStatus, TimeSpan interval,
            Scheduler scheduler)
        {
            this.report = report;
            this.metricsDataProvider = metricsDataProvider;
            this.healthStatus = healthStatus;
            this.scheduler = scheduler;
            this.scheduler.Start(interval, t => RunReport(t));
        }

        public void Dispose()
        {
            using (this.scheduler)
            {
            }
            using (this.report as IDisposable)
            {
            }
        }

        private void RunReport(CancellationToken token)
        {
            report.RunReport(this.metricsDataProvider.CurrentMetricsData, this.healthStatus, token);
        }
    }
}