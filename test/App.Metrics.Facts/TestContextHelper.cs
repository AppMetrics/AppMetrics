using System;
using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Metrics.Facts
{
    public static class TestContextHelper
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        public static IMetricsContext Instance(string context, IClock clock, IScheduler scheduler)
        {
            Func<string, IMetricGroupRegistry> newGroupRegistry = name => new DefaultMetricGroupRegistry(name);
            var registry = new DefaultMetricsRegistry(context, SamplingType.ExponentiallyDecaying, clock,
                new EnvironmentInfo(), newGroupRegistry);
            return new DefaultMetricsContext(context, clock, registry,
                new TestMetricsBuilder(clock, scheduler),
                new DefaultHealthCheckManager(LoggerFactory,
                    new DefaultHealthCheckRegistry(LoggerFactory, Enumerable.Empty<HealthCheck>(), Options.Create(new AppMetricsOptions()))),
                new DefaultMetricsDataManager(LoggerFactory, registry));
        }

        public static IMetricsContext Instance()
        {
            return Instance("TestContext", new Clock.TestClock());
        }

        public static IMetricsContext Instance(IClock clock, IScheduler scheduler)
        {
            return Instance("TestContext", clock, scheduler);
        }

        public static IMetricsContext Instance(string context, IClock clock)
        {
            return Instance("TestContext", clock, new TestScheduler(clock));
        }
    }
}