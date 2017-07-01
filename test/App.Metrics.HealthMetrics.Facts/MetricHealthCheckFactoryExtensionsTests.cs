// <copyright file="MetricHealthCheckFactoryExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.HealthMetrics.Facts.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.HealthMetrics.Facts
{
    public class MetricHealthCheckFactoryExtensionsTests : IClassFixture<MetricsFixture>
    {
        private readonly MetricsFixture _fixture;

        public MetricHealthCheckFactoryExtensionsTests(MetricsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Can_register_and_execute_apdex_check()
        {
            var name = "apdex health check";

            _fixture.Metrics.Measure.Apdex.Track(TestMetricsRegistry.RequestsApdex, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 101));

            _fixture.HealthCheckRegistry.AddMetricCheck(
                name: name,
                metrics: _fixture.Metrics,
                options: TestMetricsRegistry.RequestsApdex,
                passing: value => { return (message: $"OK. Apdex Score < 0.5 ({value.Score})", result: value.Score < 0.5); },
                warning: value => { return (message: $"WARNING. Apdex Score between 0.5 and 0.75 ({value.Score})", result: value.Score < 0.75); },
                failing: value => { return (message: $"FAILED. Apdex Score >= 0.75 ({value.Score})", result: value.Score >= 0.75); });

            var check = _fixture.HealthCheckRegistry.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
            result.Check.Message.Should().StartWith("FAILED. Apdex Score >= 0.75");
        }

        [Fact]
        public async Task Can_register_and_execute_counter_check()
        {
            var name = "counter health check";

            _fixture.Metrics.Measure.Counter.Increment(TestMetricsRegistry.CacheMisses, 3);

            _fixture.HealthCheckRegistry.AddMetricCheck(
                name: name,
                metrics: _fixture.Metrics,
                options: TestMetricsRegistry.CacheMisses,
                passing: value => { return (message: $"OK. Cache Misses < 5 ({value.Count})", result: value.Count < 5); },
                warning: value => { return (message: $"WARNING. Cache Misses < 7 ({value.Count})", result: value.Count < 200); },
                failing: value => { return (message: $"FAILED. Cache Misses >=7 ({value.Count})", result: value.Count >= 7); });

            var check = _fixture.HealthCheckRegistry.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. Cache Misses < 5 ");
        }

        [Fact]
        public async Task Can_register_and_execute_histogram_check()
        {
            var name = "histogram health check";

            _fixture.Metrics.Measure.Histogram.Update(TestMetricsRegistry.PostRequestSize, 10);

            _fixture.HealthCheckRegistry.AddMetricCheck(
                name: name,
                metrics: _fixture.Metrics,
                options: TestMetricsRegistry.PostRequestSize,
                passing: value => { return (message: $"OK. 98th Percentile < 100kb ({value.Percentile98})",result: value.Percentile98 < 100); },
                warning: value => { return (message: $"WARNING. 98th Percentile > 100kb ({value.Percentile98})", result: value.Percentile98 < 200); },
                failing: value => { return (message: $"FAILED. 98th Percentile > 200kb ({value.Percentile98})", result: value.Percentile98 > 200); });

            var check = _fixture.HealthCheckRegistry.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. 98th Percentile < 100kb");
        }

        [Fact]
        public async Task Can_register_and_execute_meter_check()
        {
            var name = "meter health check";

            _fixture.Metrics.Measure.Meter.Mark(TestMetricsRegistry.FailedLoginRate, 10);

            _fixture.HealthCheckRegistry.AddMetricCheck(
                name: name,
                metrics: _fixture.Metrics,
                options: TestMetricsRegistry.FailedLoginRate,
                passing: value => { return (message: $"OK. Failed Login Rate Normal ({value.OneMinuteRate})", result: value.OneMinuteRate < 50.0); },
                warning: value => { return (message: $"WARNING. Could be an issue with login ({value.OneMinuteRate})", result: value.OneMinuteRate < 70.0); },
                failing: value => { return (message: $"FAILED. Failed logins more than expected ({value.OneMinuteRate})" , result: value.OneMinuteRate >= 70.0); });

            var check = _fixture.HealthCheckRegistry.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
            result.Check.Message.Should().StartWith("OK. Failed Login Rate Normal");
        }

        [Fact]
        public async Task Can_register_and_execute_timer_check()
        {
            var name = "timer health check";

            _fixture.Metrics.Measure.Timer.Time(TestMetricsRegistry.DatabaseTimer, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 101));

            _fixture.HealthCheckRegistry.AddMetricCheck(
                name: name,
                metrics: _fixture.Metrics,
                options: TestMetricsRegistry.DatabaseTimer,
                passing: value => { return (message: $"OK. 98th Percentile < 100ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 < 100); },
                warning: value => { return (message: $"WARNING. 98th Percentile > 100ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 < 200); },
                failing: value => { return (message: $"FAILED. 98th Percentile > 200ms ({value.Histogram.Percentile98}{TestMetricsRegistry.DatabaseTimer.DurationUnit.Unit()})", result: value.Histogram.Percentile98 > 200); });

            var check = _fixture.HealthCheckRegistry.Checks.FirstOrDefault(c => c.Key == name);
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Degraded);
            result.Check.Message.Should().StartWith("WARNING. 98th Percentile > 100ms");
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