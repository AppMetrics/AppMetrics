// <copyright file="HealthCheckRegistryTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckRegistryTests
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();

        private readonly HealthCheckRegistry _healthCheckRegistry = new HealthCheckRegistry();

        private readonly Func<IHealthCheckRegistry, IProvideHealth> _healthSetup;
        private IProvideHealth _health;

        public HealthCheckRegistryTests()
        {
            _healthSetup = healthCheckFactory =>
            {
                var healthManager = new DefaultHealthProvider(
                    LoggerFactory.CreateLogger<DefaultHealthProvider>(),
                    healthCheckFactory);

                _health = healthManager;

                return _health;
            };
        }

        [Fact]
        public void Registry_does_not_throw_on_duplicate_registration()
        {
            _healthCheckRegistry.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));

            Action action = () => _healthCheckRegistry.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            action.ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task Registry_status_is_degraded_if_one_check_is_degraded()
        {
            _healthCheckRegistry.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()));

            var health = _healthSetup(_healthCheckRegistry);

            var status = await health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Degraded);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_failed_if_one_check_fails()
        {
            _healthCheckRegistry.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()));

            var health = _healthSetup(_healthCheckRegistry);

            var status = await health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_healthy_if_all_checks_are_healthy()
        {
            _healthCheckRegistry.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("another", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));

            var health = _healthSetup(_healthCheckRegistry);

            var status = await health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Healthy);
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task Registry_status_is_unhealthy_if_any_one_check_fails_even_when_degraded()
        {
            _healthCheckRegistry.Register("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()));
            _healthCheckRegistry.Register("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()));

            var health = _healthSetup(_healthCheckRegistry);

            var status = await health.ReadStatusAsync();

            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Length.Should().Be(3);
        }
    }
}