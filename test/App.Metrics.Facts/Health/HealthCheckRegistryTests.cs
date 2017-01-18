// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using App.Metrics.Configuration;
using App.Metrics.Core;
using App.Metrics.Infrastructure;
using App.Metrics.Interfaces;
using App.Metrics.Internal;
using App.Metrics.Internal.Builders;
using App.Metrics.Internal.Managers;
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
                var clock = new TestClock();
                var options = new AppMetricsOptions();
                Func<string, IMetricContextRegistry> newContextRegistry = name => new DefaultMetricContextRegistry(name);
                var registry = new DefaultMetricsRegistry(
                    LoggerFactory,
                    options,
                    clock,
                    new EnvironmentInfoProvider(),
                    newContextRegistry);
                var metricBuilderFactory = new DefaultMetricsBuilderFactory();
                var filter = new DefaultMetricsFilter();
                var healthManager = new DefaultHealthManager(LoggerFactory.CreateLogger<DefaultHealthManager>(), healthCheckFactory);
                var dataManager = new DefaultDataManager(
                    filter,
                    registry);

                var metricsManagerFactory = new DefaultMetricsManagerFactory(registry, metricBuilderFactory, clock);
                var metricsManagerAdvancedFactory = new DefaultMetricsAdvancedManagerFactory(registry, metricBuilderFactory, clock);
                var metricsManager = new DefaultMetricsManager(registry, LoggerFactory.CreateLogger<DefaultMetricsManager>());

                return new DefaultMetrics(
                    options,
                    clock,
                    filter,
                    metricsManagerFactory,
                    metricBuilderFactory,
                    metricsManagerAdvancedFactory,
                    dataManager,
                    metricsManager,
                    healthManager);
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
        public async Task registry_status_is_degraded_if_one_check_is_degraded()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("degraded", () => Task.FromResult(HealthCheckResult.Degraded()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Degraded);
            status.Results.Length.Should().Be(2);
        }


        [Fact]
        public async Task registry_status_is_failed_if_one_check_fails()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("bad", () => Task.FromResult(HealthCheckResult.Unhealthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task registry_status_is_healthy_if_all_checks_are_healthy()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("another", () => Task.FromResult(HealthCheckResult.Healthy()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Healthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task registry_status_is_unhealthy_if_any_one_check_fails_even_when_degraded()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("bad", () => Task.FromResult(HealthCheckResult.Unhealthy()));
            _healthCheckFactory.Register("degraded", () => Task.FromResult(HealthCheckResult.Degraded()));

            var metrics = _metircsSetup(_healthCheckFactory);

            var status = await metrics.Health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(3);
        }
    }
}