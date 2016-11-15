using System;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Utils;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Performance.Tests.Setup
{
    [MarkdownExporter]
    public abstract class MetricBenchmarkSetup
    {
        public IMetrics Metrics;

        [Setup]
        public void Setup()
        {
            var loggerFactory = new LoggerFactory();
            var metricsLogger = loggerFactory.CreateLogger<DefaultAdvancedMetrics>();
            var healthFactoryLogger = loggerFactory.CreateLogger<HealthCheckFactory>();
            var clock = new TestClock();
            var options = new AppMetricsOptions { DefaultSamplingType = SamplingType.LongTerm };
            Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
            var registry = new DefaultMetricsRegistry(loggerFactory, options, clock, new EnvironmentInfoProvider(loggerFactory), newContextRegistry);
            var healthCheckFactory = new HealthCheckFactory(healthFactoryLogger);
            var advancedContext = new DefaultAdvancedMetrics(metricsLogger, options, clock, new DefaultMetricsFilter(), registry, healthCheckFactory);
            Metrics = new DefaultMetrics(options, registry, advancedContext);
        }
    }
}