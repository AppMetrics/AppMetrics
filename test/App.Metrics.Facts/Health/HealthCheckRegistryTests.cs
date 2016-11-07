using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class HealthCheckRegistryTests
    {
        private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();
        private readonly HealthCheckFactory _healthCheckFactory;
        private readonly IHealthCheckManager _healthCheckManager;
        private readonly List<HealthCheck> _healthChecks = new List<HealthCheck>();

        public HealthCheckRegistryTests()
        {
            _healthCheckFactory = new HealthCheckFactory(_healthChecks);
            _healthCheckManager = new DefaultHealthCheckManager(LoggerFactory,
                () => _healthCheckFactory.Checks);
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

            var status = await _healthCheckManager.GetStatusAsync();

            status.IsHealthy.Should().BeFalse();
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task registry_status_is_healthy_if_all_checks_are_healthy()
        {
            _healthCheckFactory.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckFactory.Register("another", () => Task.FromResult(HealthCheckResult.Healthy()));

            var status = await _healthCheckManager.GetStatusAsync();

            status.IsHealthy.Should().BeTrue();
            status.Results.Length.Should().Be(2);
        }
    }
}