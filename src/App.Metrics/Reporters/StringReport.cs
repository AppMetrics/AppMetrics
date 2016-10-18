using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.MetricData;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class StringReport : HumanReadableReport
    {
        private readonly IMetricsContext _metricsContext;
        private StringBuilder _buffer;
        private bool _disposed = false;


        public StringReport(ILoggerFactory loggerFactory,
            IMetricsContext metricsContext,
            IMetricsFilter filter)
            : base(loggerFactory, filter, metricsContext.Advanced.Clock)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            _metricsContext = metricsContext;
        }

        public string Result => _buffer.ToString();

        public async Task<string> RenderMetrics(IMetricsContext metricsContext)
        {
            await RunReport(metricsContext, CancellationToken.None);

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