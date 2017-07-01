// <copyright file="HealthCheckFactoryExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckFactoryExtensionsTests
    {
        private readonly ILogger<HealthCheckRegistry> _logger;

        public HealthCheckFactoryExtensionsTests() { _logger = new LoggerFactory().CreateLogger<HealthCheckRegistry>(); }

        [Fact]
        [Trait("Category", "Requires Connectivity")]
        public async Task Can_execute_http_get_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github home";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddHttpGetCheck(name, new Uri("https://github.com"), TimeSpan.FromSeconds(10));

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

#pragma warning disable xUnit1004 // Test methods should not be skipped
        [Fact(Skip = "Mock HTTP Call")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
        [Trait("Category", "Requires Connectivity")]
        public async Task Can_execute_ping_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github ping";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddPingCheck(name, "github.com", TimeSpan.FromSeconds(10));

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessPhysicalMemoryCheck(name, int.MaxValue);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessPrivateMemorySizeCheck(name, int.MaxValue);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessVirtualMemorySizeCheck(name, long.MaxValue);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public void Can_register_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessPhysicalMemoryCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessPrivateMemorySizeCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.AddProcessVirtualMemorySizeCheck(name, 100);

            registry.Checks.Should().NotBeEmpty();
            registry.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public async Task Should_be_unhealthy_when_task_is_cancelled()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "custom with cancellation token";

            var registry = new HealthCheckRegistry(healthChecks);

            registry.Register(
                name,
                async cancellationToken =>
                {
                    await Task.Delay(2000, cancellationToken);
                    return HealthCheckResult.Healthy();
                });

            var token = new CancellationTokenSource();
            token.CancelAfter(200);

            var check = registry.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync(token.Token).ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }
    }
}