using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.DataProviders;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class StringReport : HumanReadableReport
    {
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;
        private StringBuilder _buffer;
        private bool _disposed = false;


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

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources                    
                }

                // Release unmanaged resources.
                // Set large fields to null.
                _buffer = null;
                _disposed = true;
            }

            base.Dispose(disposing);
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