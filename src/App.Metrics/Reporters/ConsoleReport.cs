using System;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporters
{
    public sealed class ConsoleReport : HumanReadableReport
    {
        private bool _disposed = false;

        public ConsoleReport(ILoggerFactory loggerFactory, IClock systemClock)
            : base(loggerFactory, systemClock)
        {
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