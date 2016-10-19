using System;
using System.Diagnostics;
using App.Metrics.MetricData;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class ConsoleReport : HumanReadableReport
    {
        private readonly IMetricsContext _metricsContext;
        private bool _disposed = false;
        private readonly ILogger _logger;

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
            _logger = loggerFactory.CreateLogger<ConsoleReport>();
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
            _logger.ReportStarting<ConsoleReport>();

            var startTimestamp = _logger.IsEnabled(LogLevel.Information) ? Stopwatch.GetTimestamp() : 0;

            Console.Clear();

            base.StartReport(contextName);

            _logger.ReportedStarted<ConsoleReport>(startTimestamp);
        }

        protected override void WriteLine(string line, params string[] args)
        {
            Console.WriteLine(line, args);
        }
    }
}