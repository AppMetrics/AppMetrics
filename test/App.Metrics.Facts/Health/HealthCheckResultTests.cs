using System;
using App.Metrics.Core;
using App.Metrics.Health;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class HealthCheckResultTests
    {
        [Fact]
        public void can_create_degraded()
        {
            var result = HealthCheckResult.Degraded("degrading api");

            result.Message.Should().Be("degrading api");
            result.Status.Should().Be(HealthCheckStatus.Degraded);
        }

        [Fact]
        public void can_create_degraded_with_exception_info()
        {
            var exception = new InvalidOperationException();
            var exceptionString = $"EXCEPTION: {exception.GetType().Name} - {exception.Message}";
            var result = HealthCheckResult.Degraded(exception);

            result.Message.Should().StartWith(exceptionString);
            result.Status.Should().Be(HealthCheckStatus.Degraded);
        }

        [Fact]
        public void can_create_degraded_with_formatted_values()
        {
            var id = Guid.NewGuid();
            var result = HealthCheckResult.Degraded("degrading api {0}", id);

            result.Message.Should().Be($"degrading api {id}");
            result.Status.Should().Be(HealthCheckStatus.Degraded);
        }

        [Fact]
        public void can_create_ignored()
        {
            var result = HealthCheckResult.Ignore();

            result.Message.Should().Be("ignored check");
            result.Status.Should().Be(HealthCheckStatus.Ignored);
        }

        [Fact]
        public void message_defaults_to_degraded()
        {
            var id = Guid.NewGuid();
            var result = HealthCheckResult.Degraded();

            result.Message.Should().Be("DEGRADED");
            result.Status.Should().Be(HealthCheckStatus.Degraded);
        }
    }
}