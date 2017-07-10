// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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

        [Theory]
        [Trait("Category", "Requires Connectivity")]
        [InlineData(HealthCheckStatus.Healthy, "https://github.com")]
        [InlineData(HealthCheckStatus.Degraded, "https://github.unknown", true)]
        [InlineData(HealthCheckStatus.Unhealthy, "https://github.unknown", false)]
        public async Task can_execute_http_get_check(HealthCheckStatus expectedResult, string uriString, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github home";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterHttpGetHealthCheck(name, new Uri(uriString), TimeSpan.FromSeconds(5), degradedOnError: degradedOnError);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory(Skip = "Mock HTTP Call")]
        [Trait("Category", "Requires Connectivity")]
        [InlineData(HealthCheckStatus.Healthy, "github.com")]
        [InlineData(HealthCheckStatus.Degraded, "github.unknown", true)]
        [InlineData(HealthCheckStatus.Unhealthy, "github.unknown", false)]
        public async Task can_execute_ping_check(HealthCheckStatus expectedResult, string host, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "github ping";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterPingHealthCheck(name, host, TimeSpan.FromSeconds(5), degradedOnError: degradedOnError);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task can_execute_process_physical_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, thresholdBytes, degradedOnError);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, int.MaxValue)]
        [InlineData(HealthCheckStatus.Degraded, int.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, int.MinValue, false)]
        public async Task can_execute_process_private_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, thresholdBytes, degradedOnError);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(HealthCheckStatus.Healthy, long.MaxValue)]
        [InlineData(HealthCheckStatus.Degraded, long.MinValue, true)]
        [InlineData(HealthCheckStatus.Unhealthy, long.MinValue, false)]
        public async Task can_execute_process_virtual_memory_check(HealthCheckStatus expectedResult, long thresholdBytes, bool degradedOnError = false)
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessVirtualMemorySizeHealthCheck(name, thresholdBytes, degradedOnError);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(expectedResult);
        }

        [Fact]
        public void can_register_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void can_register_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, new Lazy<IMetrics>(), healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void can_register_process_virtual_memory_check()
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