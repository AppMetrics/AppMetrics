using System;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsFixture()
        {
            var options = new AppMetricsOptions {Clock = new Clock.TestClock(), DefaultSamplingType = SamplingType.LongTerm};
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(_loggerFactory, options, new EnvironmentInfoBuilder(_loggerFactory), newContextRegistry);
            var healthCheckFactory = new HealthCheckFactory();
            var advancedContext = new DefaultAdvancedMetrics(options, new DefaultMetricsFilter(), registry, healthCheckFactory);
            Metrics = new DefaultMetrics(options, registry, advancedContext);
        }

        public IMetrics Metrics { get; }

        public Func<IMetrics, Task<MetricsDataValueSource>> CurrentData =>
            async ctx => await Metrics.Advanced.Data.ReadDataAsync();

        public Func<IMetrics, IMetricsFilter, Task<MetricsDataValueSource>> CurrentDataWithFilter
            => async (ctx, filter) => await Metrics.Advanced.Data.ReadDataAsync(filter);

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            Metrics?.Advanced.Data.Reset();
        }
    }
}