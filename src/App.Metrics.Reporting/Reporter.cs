using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Reporting
{
    internal sealed class Reporter : IReporter
    {
        private readonly string _name;
        private readonly ReportFactory _reportFactory;
        private IReporter[] _reporters;

        public Reporter(ReportFactory reportFactory, string name)
        {
            _reportFactory = reportFactory;
            _name = name;

            var providers = reportFactory.GetProviders();
            if (providers.Length > 0)
            {
                _reporters = new IReporter[providers.Length];
                for (var index = 0; index < providers.Length; index++)
                {
                    _reporters[index] = providers[index].CreateReporter(name);
                }
            }
        }

        public Task RunReports(IMetricsContext context, CancellationToken token)
        {
            if (_reporters == null)
            {
                return AppMetricsTaskCache.EmptyTask;
            }

            List<Exception> exceptions = null;
            foreach (var reporter in _reporters)
            {
                try
                {
                    reporter.RunReports(context, token);
                }
                catch (Exception ex)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }

                    exceptions.Add(ex);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(
                    message: "An error occurred while running reporter(s).", innerExceptions: exceptions);
            }

            return AppMetricsTaskCache.EmptyTask;
        }

        internal void AddProvider(IReporterProvider provider)
        {
            var reporter = provider.CreateReporter(_name);
            int logIndex;
            if (_reporters == null)
            {
                logIndex = 0;
                _reporters = new IReporter[1];
            }
            else
            {
                logIndex = _reporters.Length;
                Array.Resize(ref _reporters, logIndex + 1);
            }
            _reporters[logIndex] = reporter;
        }
    }
}