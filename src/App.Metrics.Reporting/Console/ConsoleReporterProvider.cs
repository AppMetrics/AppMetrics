using System;
using System.Collections.Concurrent;
using App.Metrics.Internal;
using Serilog;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterProvider : IReporterProvider
    {
        private readonly ConcurrentDictionary<string, ConsoleReporter> _reporters = new ConcurrentDictionary<string, ConsoleReporter>();
        private readonly IMetricsFilter _filter;
        private readonly IConsoleReporterSettings _settings;
        private readonly ILogger _logger;

        public ConsoleReporterProvider(IConsoleReporterSettings settings,  IMetricsFilter filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (filter == null)
            {
                throw new ArgumentException(nameof(filter));
            }

            _filter = filter;
            _settings = settings;

            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public ConsoleReporterProvider(IConsoleReporterSettings settings)
            : this(settings, new NoOpFilter())
        {

        }

        public IReporter CreateReporter(string name)
        {
            return _reporters.GetOrAdd(name, CreateReporterImplementation);
        }

        private ConsoleReporter CreateReporterImplementation(string name)
        {
            return new ConsoleReporter(name, _settings.Interval, _settings.Disabled, _filter);
        }

        public void Dispose()
        {
        }
    }
}