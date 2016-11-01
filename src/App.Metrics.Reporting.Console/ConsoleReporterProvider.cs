using System;
using System.Collections.Concurrent;

namespace App.Metrics.Reporting
{
    public class ConsoleReporterProvider : IReporterProvider
    {
        private readonly ConcurrentDictionary<string, ConsoleReporter> _reporters = new ConcurrentDictionary<string, ConsoleReporter>();
        private readonly IConsoleReporterSettings _settings;

        public ConsoleReporterProvider(IConsoleReporterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
        }

        public IMetricReporter CreateMetricReporter(string name)
        {
            return _reporters.GetOrAdd(name, CreateReporterImplementation);
        }

        public void Dispose()
        {
        }

        private ConsoleReporter CreateReporterImplementation(string name)
        {
            return new ConsoleReporter(name);
        }
    }
}