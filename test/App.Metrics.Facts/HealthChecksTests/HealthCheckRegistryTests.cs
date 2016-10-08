using System;
using System.Threading.Tasks;
using App.Metrics.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Internal;
using Xunit;

namespace App.Metrics.Facts.HealthChecksTests
{
    public class HealthCheckRegistryTests
    {
        private HealthChecks _healthCheckRegistry;

        public HealthCheckRegistryTests()
        {
            _healthCheckRegistry = new HealthChecks();    
        }

        [Fact]
        public void HealthCheck_RegistryDoesNotThrowOnDuplicateRegistration()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Healthy())));

            Action action = () => _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Healthy())));
            action.ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task HealthCheck_RegistryExecutesCheckOnEachGetStatus()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();
            var count = 0;

            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("test", () =>
            {
                count++;
                return Task.FromResult(HealthCheckResult.Healthy());
            }));

            count.Should().Be(0);

            await _healthCheckRegistry.GetStatusAsync();

            count.Should().Be(1);

            await _healthCheckRegistry.GetStatusAsync();

            count.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsFailedIfOneCheckFails()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("ok", () => Task.FromResult(HealthCheckResult.Healthy())));
            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("bad", () => Task.FromResult(HealthCheckResult.Unhealthy())));

            var status = await _healthCheckRegistry.GetStatusAsync();

            status.IsHealthy.Should().BeFalse();
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsHealthyIfAllChecksAreHealthy()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("ok", () => Task.FromResult(HealthCheckResult.Healthy())));
            _healthCheckRegistry.RegisterHealthCheck(new HealthCheck("another", () => Task.FromResult(HealthCheckResult.Healthy())));

            var status = await _healthCheckRegistry.GetStatusAsync();

            status.IsHealthy.Should().BeTrue();
            status.Results.Length.Should().Be(2);
        }
    }
}