using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class DefaultMetricsContextTestFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public IMetrics Context { get; }

        public Func<IMetrics, Task<MetricsDataValueSource>> CurrentData => 
            async ctx => await Context.Advanced.DataManager.GetAsync();

        public Func<IMetrics, IMetricsFilter, Task<MetricsDataValueSource>> CurrentDataWithFilter
            => async (ctx, filter) => await Context.Advanced.DataManager.GetAsync(filter);

        public DefaultMetricsContextTestFixture()
        {
            var options = new AppMetricsOptions();
            var healthCheckManager = new DefaultHealthCheckManager(_loggerFactory, () => new ConcurrentDictionary<string, HealthCheck>());
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(_loggerFactory, options, new EnvironmentInfoBuilder(_loggerFactory), newContextRegistry);
            var metricsDataManager = new DefaultMetricsDataManager(registry);
            var advancedContext = new DefaultAdancedMetrics(options, registry, healthCheckManager, metricsDataManager);
            Context = new DefaultMetrics(options, registry, advancedContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            Context?.Advanced.DataManager.Reset();
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}