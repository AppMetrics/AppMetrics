using System;
using System.Text;
using System.Threading;
using App.Metrics.MetricData;

namespace App.Metrics.Reporters
{
    public class StringReport : HumanReadableReport
    {
        private StringBuilder buffer;

        public string Result => this.buffer.ToString();

        public static string RenderMetrics(MetricsData metricsData, Func<HealthStatus> healthStatus)
        {
            var report = new StringReport();
            report.RunReport(metricsData, healthStatus, CancellationToken.None);
            return report.Result;
        }

        protected override void StartReport(string contextName)
        {
            this.buffer = new StringBuilder();
            base.StartReport(contextName);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            this.buffer.AppendLine(string.Format(line, args));
        }
    }
}