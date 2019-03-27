// <copyright file="HealthCheckRunnerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckRunnerTests
    {
        [Fact]
        public async Task Status_is_degraded_if_one_check_is_degraded()
        {
            // Arrange
            var checks = new[]
                         {
                            new HealthCheck("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())),
                            new HealthCheck("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()))
                         };
            var runner = new DefaultHealthCheckRunner(checks);

            // Act
            var status = await runner.ReadAsync();

            // Assert
            status.Status.Should().Be(HealthCheckStatus.Degraded);
            status.Results.Count().Should().Be(2);
        }

        [Fact]
        public async Task Status_is_failed_if_one_check_fails()
        {
            // Arrange
            var checks = new[]
                         {
                             new HealthCheck("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())),
                             new HealthCheck("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()))
                         };
            var runner = new DefaultHealthCheckRunner(checks);

            // Act
            var status = await runner.ReadAsync();

            // Assert
            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Count().Should().Be(2);
        }

        [Fact]
        public async Task Status_is_healthy_if_all_checks_are_healthy()
        {
            // Arrange
            var checks = new[]
                         {
                             new HealthCheck("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())),
                             new HealthCheck("another", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                         };
            var runner = new DefaultHealthCheckRunner(checks);

            // Act
            var status = await runner.ReadAsync();

            // Assert
            status.Status.Should().Be(HealthCheckStatus.Healthy);
            status.Results.Count().Should().Be(2);
        }

        [Fact]
        public async Task Status_is_unhealthy_if_any_one_check_fails_even_when_degraded()
        {
            // Arrange
            var checks = new[]
                         {
                             new HealthCheck("ok", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())),
                             new HealthCheck("bad", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy())),
                             new HealthCheck("degraded", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded()))
                         };
            var runner = new DefaultHealthCheckRunner(checks);

            // Act
            var status = await runner.ReadAsync();

            // Assert
            status.Status.Should().Be(HealthCheckStatus.Unhealthy);
            status.Results.Count().Should().Be(3);
        }
    }
}
