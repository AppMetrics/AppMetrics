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
        [Fact]
        public void HealthCheck_RegistryDoesNotThrowOnDuplicateRegistration()
        {
            HealthChecks.UnregisterAllHealthChecks();

            HealthChecks.RegisterHealthCheck(new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Healthy())));

            Action action = () => HealthChecks.RegisterHealthCheck(new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Healthy())));
            action.ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public async Task HealthCheck_RegistryExecutesCheckOnEachGetStatus()
        {
            HealthChecks.UnregisterAllHealthChecks();
            var count = 0;

            HealthChecks.RegisterHealthCheck(new HealthCheck("test", () =>
            {
                count++;
                return Task.FromResult(HealthCheckResult.Healthy());
            }));

            count.Should().Be(0);

            await HealthChecks.GetStatus();

            count.Should().Be(1);

            await HealthChecks.GetStatus();

            count.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsFailedIfOneCheckFails()
        {
            HealthChecks.UnregisterAllHealthChecks();

            HealthChecks.RegisterHealthCheck(new HealthCheck("ok", () => Task.FromResult(HealthCheckResult.Healthy())));
            HealthChecks.RegisterHealthCheck(new HealthCheck("bad", () => Task.FromResult(HealthCheckResult.Unhealthy())));

            var status = await HealthChecks.GetStatus();

            status.IsHealthy.Should().BeFalse();
            status.Results.Length.Should().Be(2);
        }

        [Fact]
        public async Task HealthCheck_RegistryStatusIsHealthyIfAllChecksAreHealthy()
        {
            HealthChecks.UnregisterAllHealthChecks();

            HealthChecks.RegisterHealthCheck(new HealthCheck("ok", () => Task.FromResult(HealthCheckResult.Healthy())));
            HealthChecks.RegisterHealthCheck(new HealthCheck("another", () => Task.FromResult(HealthCheckResult.Healthy())));

            var status = await HealthChecks.GetStatus();

            status.IsHealthy.Should().BeTrue();
            status.Results.Length.Should().Be(2);
        }
    }
}