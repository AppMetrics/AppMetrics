using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.DataProviders;
using App.Metrics.Registries;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Metrics.Facts.HealthChecksTests
{
    public class HealthCheckRegistryTests
    {
        private readonly IHealthCheckRegistry _healthCheckRegistry;
        private readonly IHealthCheckDataProvider _healthCheckDataProvider;

        public HealthCheckRegistryTests()
        {
            _healthCheckRegistry = new HealthCheckRegistry(Enumerable.Empty<HealthCheck>(),
                Options.Create(new AppMetricsOptions()));
            _healthCheckDataProvider = new HealthCheckDataProvider(_healthCheckRegistry);
        }

        [Fact]
        public void HealthCheck_RegistryDoesNotThrowOnDuplicateRegistration()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));

            Action action = () => _healthCheckRegistry.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));
            action.ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task HealthCheck_RegistryExecutesCheckOnEachGetStatus()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();
            var count = 0;

            _healthCheckRegistry.Register("test", () =>
            {
                count++;
                return Task.FromResult(HealthCheckResult.Healthy());
            });

            count.Should().Be(0);

            await _healthCheckDataProvider.GetStatusAsync();

            count.Should().Be(1);

            await _healthCheckDataProvider.GetStatusAsync();

            count.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsFailedIfOneCheckFails()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("bad", () => Task.FromResult(HealthCheckResult.Unhealthy()));

            var status = await _healthCheckDataProvider.GetStatusAsync();

            status.IsHealthy.Should().BeFalse();
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsHealthyIfAllChecksAreHealthy()
        {
            _healthCheckRegistry.UnregisterAllHealthChecks();

            _healthCheckRegistry.Register("ok", () => Task.FromResult(HealthCheckResult.Healthy()));
            _healthCheckRegistry.Register("another", () => Task.FromResult(HealthCheckResult.Healthy()));

            var status = await _healthCheckDataProvider.GetStatusAsync();

            status.IsHealthy.Should().BeTrue();
            status.Results.Length.Should().Be(2);
        }
    }
}