using System;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class HealthCheckRegistryTests
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();
        private readonly HealthCheckFactory _healthCheckFactory = new HealthCheckFactory(LoggerFactory.CreateLogger<HealthCheckFactory>());
        private readonly Func<IHealthCheckFactory, IMetrics> _metircsSetup;

        public HealthCheckRegistryTests()
        {
            _metircsSetup = healthCheckFactory =>
            {
                var metricsLogger = LoggerFactory.CreateLogger<DefaultAdvancedMetrics>();
                var clock = new TestClock();
                var options = new AppMetricsOptions();
                Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
                var registry = new DefaultMetricsRegistry(LoggerFactory, options, clock, new EnvironmentInfoBuilder(LoggerFactory), newContextRegistry);
                var advancedContext = new DefaultAdvancedMetrics(metricsLogger, options, clock, new DefaultMetricsFilter(), registry, healthCheckFactory);
                return new DefaultMetrics(options, registry, advancedContext);
            };
        }

        [Fact]
        public void registry_does_not_throw_on_duplicate_registration()
        {
            _healthCheckFactory.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));

            Action action = () => _healthCheckFactory.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));
            action.ShouldNotThrow<InvalidOperationException>();
        }


        [Fact]
        public async Task registry_status_is_failed_if_one_check_fails()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("bad", () => Task.FromResult(HealthCheckResult.Unhealthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Advanced.Health.ReadStatusAsync();

            status.IsHealthy.Should().BeFalse();
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task registry_status_is_healthy_if_all_checks_are_healthy()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("another", () => Task.FromResult(HealthCheckResult.Healthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Advanced.ReadStatusAsync();

            status.IsHealthy.Should().BeTrue();
            status.Results.Length.Should().Be(2);
        }
    }
}