#region copyright

// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#endregion

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
        public async Task can_execute_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, int.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task can_execute_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, int.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task can_execute_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessVirtualMemorySizeHealthCheck(name, int.MaxValue);

            var check = factory.Checks.FirstOrDefault();
            var result = await check.Value.ExecuteAsync().ConfigureAwait(false);

            result.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public void can_register_process_physical_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "physical memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessPhysicalMemoryHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void can_register_process_private_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "private memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessPrivateMemorySizeHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }

        [Fact]
        public void can_register_process_virtual_memory_check()
        {
            var healthChecks = Enumerable.Empty<HealthCheck>();
            var name = "virtual memory";

            var factory = new HealthCheckFactory(_logger, healthChecks);

            factory.RegisterProcessVirtualMemorySizeHealthCheck(name, 100);

            factory.Checks.Should().NotBeEmpty();
            factory.Checks.Single().Value.Name.Should().Be(name);
        }
    }
}