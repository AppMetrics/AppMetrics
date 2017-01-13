using System.Linq;
using App.Metrics.Core;
using App.Metrics.Internal;
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