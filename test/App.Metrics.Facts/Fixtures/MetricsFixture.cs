using System;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public MetricsFixture()
        {
            var metricsLogger = _loggerFactory.CreateLogger<DefaultAdvancedMetrics>();
            var healthFactoryLogger = _loggerFactory.CreateLogger<HealthCheckFactory>();
            var clock = new TestClock();
            var options = new AppMetricsOptions { DefaultSamplingType = SamplingType.LongTerm };
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(_loggerFactory, options, clock, new EnvironmentInfoProvider(), newContextRegistry);
            var healthCheckFactory = new HealthCheckFactory(healthFactoryLogger);
            var advancedContext = new DefaultAdvancedMetrics(metricsLogger, options, clock, new DefaultMetricsFilter(), registry, healthCheckFactory);
            Metrics = new DefaultMetrics(options, registry, advancedContext);
        }

        public Func<IMetrics, MetricsDataValueSource> CurrentData =>
            ctx => Metrics.Advanced.Data.ReadData();

        public Func<IMetrics, IMetricsFilter, MetricsDataValueSource> CurrentDataWithFilter
            => (ctx, filter) => Metrics.Advanced.Data.ReadData(filter);

        public IMetrics Metrics { get; }

        public void Dispose() { Dispose(true); }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Metrics?.Advanced.Data.Reset();
        }
    }
}