using System;
using System.Text;
using System.Threading;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public class StringReport : HumanReadableReport
    {
        private StringBuilder _buffer;

        public StringReport(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public string Result => _buffer.ToString();

        public string RenderMetrics(MetricsData metricsData, Func<HealthStatus> healthStatus)
        {
            RunReport(metricsData, healthStatus, CancellationToken.None);
            return Result;
        }

        protected override void StartReport(string contextName)
        {
            _buffer = new StringBuilder();
            base.StartReport(contextName);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            _buffer.AppendLine(string.Format(line, args));
        }
    }
}