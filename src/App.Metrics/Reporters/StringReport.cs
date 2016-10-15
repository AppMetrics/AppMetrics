using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public class StringReport : HumanReadableReport
    {
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;
        private StringBuilder _buffer;

        public StringReport(ILoggerFactory loggerFactory,
            IHealthCheckDataProvider healthCheckDataProvider)
            : base(loggerFactory)
        {
            _healthCheckDataProvider = healthCheckDataProvider;
        }

        public string Result => _buffer.ToString();

        public async Task<string> RenderMetrics(MetricsData metricsData)
        {
            await RunReport(metricsData, _healthCheckDataProvider, CancellationToken.None);
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