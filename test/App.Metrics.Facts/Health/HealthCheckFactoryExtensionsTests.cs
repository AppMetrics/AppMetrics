// <copyright file="HealthCheckFactoryExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Internal;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class HealthCheckFactoryExtensionsTests
    {
        private readonly ILogger<HealthCheckFactory> _logger;

        public HealthCheckFactoryExtensionsTests() { _logger = new LoggerFactory().CreateLogger<HealthCheckFactory>(); }

        [Fact]
        [Trait("Category", "Requires Connectivity")]
        public async Task Can_execute_http_get_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github home";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterHttpGetHealthCheck(name, new Uri("https://github.com"), TimeSpan.FromSeconds(10));

            var check = factory.Checks.FirstOrDefault();
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

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterPingHealthCheck(name, "github.com", TimeSpan.FromSeconds(10));

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, int.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, int.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Can_execute_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessVirtualMemorySizeHealthCheck(name, long.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public void Can_register_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void Can_register_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessVirtualMemorySizeHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }
    }
}