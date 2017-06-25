// <copyright file="HealthCheckRegistryTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Filtering;
using App.Metrics.Core.Infrastructure;
using App.Metrics.Core.Internal;
using App.Metrics.Health.Internal;
using App.Metrics.Registry;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckRegistryTests
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        private readonly HealthCheckFactory _healthCheckFactory =
            new HealthCheckFactory(LoggerFactory.CreateLogger<HealthCheckFactory>(), new Lazy<IMetrics>());

        private readonly Func<IHealthCheckFactory, IMetrics> _metircsSetup;
        private IMetrics _metrics;

        public HealthCheckRegistryTests()
        {
            _metircsSetup = healthCheckFactory =>
            {
                var clock = new TestClock();
                var options = new AppMetricsOptions();

                IMetricContextRegistry NewContextRegistry(string name) => new DefaultMetricContextRegistry(name);

                var registry = new DefaultMetricsRegistry(
                    LoggerFactory,
                    options,
                    clock,
                    NewContextRegistry);
                var metricBuilderFactory = new DefaultMetricsBuilderFactory();
                var filter = new DefaultMetricsFilter();
                var healthManager = new DefaultHealthProvider(
                    new Lazy<IMetrics>(() => _metrics),
                    LoggerFactory.CreateLogger<DefaultHealthProvider>(),
                    healthCheckFactory);
                var dataManager = new DefaultMetricValuesProvider(
                    filter,
                    registry);

                var metricsManagerFactory = new DefaultMeasureMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManagerAdvancedFactory = new DefaultMetricsProvider(registry, metricBuilderFactory, clock);
                var metricsManager = new DefaultMetricsManager(registry, LoggerFactory.CreateLogger<DefaultMetricsManager>());

                _metrics = new DefaultMetrics(
                    clock,
                    filter,
                    metricsManagerFactory,
                    metricBuilderFactory,
                    metricsManagerAdvancedFactory,
                    dataManager,
                    metricsManager,
                    healthManager);

                return _metrics;
            };
        }

        [Fact]
        public void Registry_does_not_throw_on_duplicate_registration()
        {
            _healthCheckFactory.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));

            Action action = () => _healthCheckFactory.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            action.ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task Registry_status_is_degraded_if_one_check_is_degraded()
        {
            _healthCheckFactory.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Degraded);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_failed_if_one_check_fails()
        {
            _healthCheckFactory.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_healthy_if_all_checks_are_healthy()
        {
            _healthCheckFactory.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("another", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Healthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_unhealthy_if_any_one_check_fails_even_when_degraded()
        {
            _healthCheckFactory.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()));
            _healthCheckFactory.Register("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(3);
        }
    }
}