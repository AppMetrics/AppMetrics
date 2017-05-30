using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Health;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class MetricHealthCheckFactoryExtensionsTests : IClassFixture<MetricsFixture>
    {
        private readonly MetricsFixture _fixture;

        private readonly ILogger<HealthCheckFactory> _logger;

        public MetricHealthCheckFactoryExtensionsTests(MetricsFixture fixture)
        {
            _fixture = fixture;
            _logger = new LoggerFactory().CreateLogger<HealthCheckFactory>();
        }

        [Fact]
        public async Task can_register_and_execute_apdex_check()
        {
            var name = "apdex health check";

            _fixture.Metrics.Measure.Apdex.Track(TestMetricsRegistry.RequestsApdex, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 101));

            _fixture.HealthCheckFactory.RegisterMetricCheck(
                name: name,
                options: TestMetricsRegistry.RequestsApdex,
                passing: value => (message:
                    $"OK. Apdex Score < 0.5 ({value.Score})",
                    result: value.Score < 0.5),
                warning: value => (message:
                    $"WARNING. Apdex Score between 0.5 and 0.75 ({value.Score})"
                    , result: value.Score < 0.75),
                failing: value => (message:
                    $"FAILED. Apdex Score >= 0.75 ({value.Score})"
                    , result: value.Score >= 0.75));

            var check = _fixture.HealthCheckFactory.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
            result.Check.Message.Should().StartWith("FAILED. Apdex Score >= 0.75");
        }

        [Fact]
        public async Task can_register_and_execute_timer_check()
        {
            var name = "timer health check";

            _fixture.Metrics.Measure.Timer.Time(TestMetricsRegistry.DatabaseTimer, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 101));

            _fixture.HealthCheckFactory.RegisterMetricCheck(
                name: name,
                options: TestMetricsRegistry.DatabaseTimer,
                passing: value => (message:
                    $"OK. 98th Percentile < 100ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})",
                    result: value.Histogram.Percentile98 < 100),
                warning: value => (message:
                    $"WARNING. 98th Percentile > 100ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})"
                    , result: value.Histogram.Percentile98 < 200),
                failing: value => (message:
                    $"FAILED. 98th Percentile > 200ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})"
                    , result: value.Histogram.Percentile98 > 200));

            var check = _fixture.HealthCheckFactory.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Degraded);
            result.Check.Message.Should().StartWith("WARNING. 98th Percentile > 100ms");
        }

        [Fact]
        public async Task can_register_and_execute_counter_check()
        {
            var name = "counter health check";

            _fixture.Metrics.Measure.Counter.Increment(TestMetricsRegistry.CacheMisses, 3);

            _fixture.HealthCheckFactory.RegisterMetricCheck(
                name: name,
                options: TestMetricsRegistry.CacheMisses,
                passing: value => (message:
                    $"OK. Cache Misses < 5 ({value.Count})",
                    result: value.Count < 5),
                warning: value => (message:
                    $"WARNING. Cache Misses < 7 ({value.Count})"
                    , result: value.Count < 200),
                failing: value => (message:
                    $"FAILED. Cache Misses >=7 ({value.Count})"
                    , result: value.Count >= 7));

            var check = _fixture.HealthCheckFactory.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. Cache Misses < 5 ");
        }

        [Fact]
        public async Task can_register_and_execute_meter_check()
        {
            var name = "meter health check";

            _fixture.Metrics.Measure.Meter.Mark(TestMetricsRegistry.FailedLoginRate, 10);

            _fixture.HealthCheckFactory.RegisterMetricCheck(
                name: name,
                options: TestMetricsRegistry.FailedLoginRate,
                passing: value => (message:
                    $"OK. Failed Login Rate Normal ({value.OneMinuteRate})",
                    result: value.OneMinuteRate < 50.0),
                warning: value => (message:
                    $"WARNING. Could be an issue with login ({value.OneMinuteRate})"
                    , result: value.OneMinuteRate < 70.0),
                failing: value => (message:
                    $"FAILED. Failed logins more than expected ({value.OneMinuteRate})"
                    , result: value.OneMinuteRate >= 70.0));

            var check = _fixture.HealthCheckFactory.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. Failed Login Rate Normal");
        }

        [Fact]
        public async Task can_register_and_execute_histogram_check()
        {
            var name = "histogram health check";

            _fixture.Metrics.Measure.Histogram.Update(TestMetricsRegistry.PostRequestSize, 10);

            _fixture.HealthCheckFactory.RegisterMetricCheck(
                name: name,
                options: TestMetricsRegistry.PostRequestSize,
                passing: value => (message:
                    $"OK. 98th Percentile < 100kb ({value.Percentile98})",
                    result: value.Percentile98 < 100),
                warning: value => (message:
                    $"WARNING. 98th Percentile > 100kb ({value.Percentile98})"
                    , result: value.Percentile98 < 200),
                failing: value => (message:
                    $"FAILED. 98th Percentile > 200kb ({value.Percentile98})"
                    , result: value.Percentile98 > 200));

            var check = _fixture.HealthCheckFactory.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. 98th Percentile < 100kb");
        }

        private static class TestMetricsRegistry
        {
            public static readonly MeterOptions FailedLoginRate = new MeterOptions
                                                                  {

                                                                      Context = ContextName,
                                                                      Name = "Failed Login Rate"
                                                                  };

            public static readonly HistogramOptions PostRequestSize = new HistogramOptions
                                                                      {
                                                                          Context = ContextName,
                                                                          Name = "POST Request Size"
            };

            public static readonly CounterOptions CacheMisses = new CounterOptions
                                                                {
                                                                    Context = ContextName,
                                                                    Name = "Database Call",
                                                                    ResetOnReporting = false
                                                                };

            public static readonly TimerOptions DatabaseTimer = new TimerOptions
                                                                {
                                                                    Context = ContextName,
                                                                    Name = "Database Call"
                                                                };

            public static readonly ApdexOptions RequestsApdex = new ApdexOptions
                                                           {
                                                               Context = ContextName,
                                                               Name = "Requests",
                                                               AllowWarmup = false
                                                           };

            private const string ContextName = "Sandbox";
        }
    }
}