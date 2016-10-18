using System;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class ConsoleReport : HumanReadableReport
    {
        private readonly IMetricsContext _metricsContext;
        private bool _disposed = false;

        public ConsoleReport(IMetricsContext metricsContext,
            IMetricsFilter filter,
            ILoggerFactory loggerFactory)
            : base(loggerFactory, filter, metricsContext.Advanced.Clock)
        {
            if (metricsContext == null)
            {
                throw new ArgumentNullException(nameof(metricsContext));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _metricsContext = metricsContext;
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
                _disposed = true;
            }

            base.Dispose(disposing);
        }

        protected override void StartReport(string contextName)
        {
            Console.Clear();

            base.StartReport(contextName);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            Console.WriteLine(line, args);
        }
    }
}