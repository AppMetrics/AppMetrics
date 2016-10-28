using System;
using System.Collections.Concurrent;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Facts
{
    public static class TestContextHelper
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        public static IMetricsContext Instance(string defaultGroupName, IClock clock, IScheduler scheduler)
        {
            var options = Options.Create(new AppMetricsOptions
            {
                Clock = clock,
                DefaultGroupName = defaultGroupName
            });
            Func<string, IMetricGroupRegistry> newGroupRegistry = name => new DefaultMetricGroupRegistry(name);
            var registry = new DefaultMetricsRegistry(LoggerFactory, options, new EnvironmentInfoBuilder(LoggerFactory), newGroupRegistry);

            return new DefaultMetricsContext(options, registry,
                new TestMetricsBuilder(clock, scheduler),
                new DefaultHealthCheckManager(LoggerFactory, () => new ConcurrentDictionary<string, HealthCheck>()),
                new DefaultMetricsDataManager(registry));
        }

        public static IMetricsContext Instance()
        {
            return Instance("TestContext", new Clock.TestClock());
        }

        public static IMetricsContext Instance(IClock clock, IScheduler scheduler)
        {
            return Instance("TestContext", clock, scheduler);
        }

        public static IMetricsContext Instance(string defaultGroupName, IClock clock)
        {
            return Instance(defaultGroupName, clock, new TestScheduler(clock));
        }
    }
}