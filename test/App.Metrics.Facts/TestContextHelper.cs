using System.Linq;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Health;
using App.Metrics.Registries;
using App.Metrics.Utils;
using Microsoft.Extensions.Options;

namespace App.Metrics.Facts
{
    public static class TestContextHelper
    {
        public static IMetricsContext Instance(string context, IClock clock, IScheduler scheduler)
        {
            return new MetricsContext(context, clock, () => new DefaultMetricsRegistry(),
                new TestMetricsBuilder(clock, scheduler),
                new DefaultHealthCheckDataProvider(new HealthCheckRegistry(Enumerable.Empty<HealthCheck>(), Options.Create(new AppMetricsOptions()))));
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