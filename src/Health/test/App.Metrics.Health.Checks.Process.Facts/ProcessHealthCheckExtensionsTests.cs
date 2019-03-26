// <copyright file="ProcessHealthCheckExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Builder;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Checks.Process.Facts
{
    public class ProcessHealthCheckExtensionsTests
    {
        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task Can_execute_process_physical_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            // Arrange
            const string name = "physical memory";
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessPhysicalMemoryCheck(name, thresholdBytes, degradedOnError: degradedOnError);
            var health = builder.Build();

            var check = health.Checks.First();

            // Act
            var result = await check.ExecuteAsync();

            // Assert
            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task Can_execute_process_private_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            // Arrange
            const string name = "private memory";
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessPrivateMemorySizeCheck(name, thresholdBytes, degradedOnError: degradedOnError);
            var health = builder.Build();

            var check = health.Checks.First();

            // Act
            var result = await check.ExecuteAsync();

            // Assert
            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, long.MaxValue, false)]
        [InlineData(HealthCheckStatus.Degraded, long.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, long.MinValue, false)]
        public async Task Can_execute_process_virtual_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            // Arrange
            const string name = "virtual memory";
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessVirtualMemorySizeCheck(name, thresholdBytes, degradedOnError: degradedOnError);
            var health = builder.Build();

            var check = health.Checks.First();

            // Act
            var result = await check.ExecuteAsync();

            // Assert
            result.Check.Status.Should().Be(expectedResult);
        }

        [Fact]
        public void Can_register_process_physical_memory_check()
        {
            // Arrange
            const string name = "physical memory";

            // Act
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessPhysicalMemoryCheck(name, 100);
            var health = builder.Build();
            var checks = health.Checks;

            // Assert
            // ReSharper disable PossibleMultipleEnumeration
            checks.Should().NotBeNull();
            checks.Should().NotBeEmpty();
            checks.Single().Name.Should().Be(name);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Fact]
        public void Can_register_process_private_memory_check()
        {
            // Arrange
            const string name = "private memory";

            // Act
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessPrivateMemorySizeCheck(name, 100);
            var health = builder.Build();
            var checks = health.Checks;

            // Assert
            // ReSharper disable PossibleMultipleEnumeration
            checks.Should().NotBeNull();
            checks.Should().NotBeEmpty();
            checks.Single().Name.Should().Be(name);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Fact]
        public void Can_register_process_virtual_memory_check()
        {
            // Arrange
            const string name = "virtual memory";

            // Act
            var builder = new HealthBuilder()
                .HealthChecks.AddProcessVirtualMemorySizeCheck(name, 100);
            var health = builder.Build();
            var checks = health.Checks;

            // Assert
            // ReSharper disable PossibleMultipleEnumeration
            checks.Should().NotBeNull();
            checks.Should().NotBeEmpty();
            checks.Single().Name.Should().Be(name);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Fact]
        public void Should_throw_when_task_is_cancelled()
        {
            // Arrange
            const string name = "custom with cancellation token";

            // Act
            var builder = new HealthBuilder().HealthChecks.AddCheck(
                name,
                async token =>
                {
                    await Task.Delay(2000, token);
                    return HealthCheckResult.Healthy();
                });

            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(200);

            var check = builder.Build().Checks.Single();

            // Assert
            Action action = () =>
            {
                var result = check.ExecuteAsync(tokenSource.Token).GetAwaiter().GetResult();

                result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
            };

            action.Should().Throw<OperationCanceledException>();
        }
    }
}
