using System;
using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.HealthChecksTests
{
    public class HealthCheckTests
    {
        [Fact]
        public void HealthCheck_FailedAndDoesNotThrowUnhandledExceptionIfActionThrowsExceptionWithBracketsInMessage()
        {
            var name = "test";
            new HealthCheck(name, () => ThrowExceptionWithBracketsInMessage()).Execute().Check.IsHealthy.Should().BeFalse();
            new HealthCheck(name, () =>
            {
                ThrowExceptionWithBracketsInMessage();
                return "string";
            }).Execute().Check.IsHealthy.Should().BeFalse();
            new HealthCheck(name, () =>
            {
                ThrowExceptionWithBracketsInMessage();
                HealthCheckResult.Healthy();
            }).Execute().Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public void HealthCheck_FailedIfActionThrows()
        {
            var name = "test";
            new HealthCheck(name, () => ThrowException()).Execute().Check.IsHealthy.Should().BeFalse();
            new HealthCheck(name, () =>
            {
                ThrowException();
                return "string";
            }).Execute().Check.IsHealthy.Should().BeFalse();
            new HealthCheck(name, () =>
            {
                ThrowException();
                HealthCheckResult.Healthy();
            }).Execute().Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public void HealthCheck_FailedIfResultUnhealthy()
        {
            new HealthCheck("test", () => HealthCheckResult.Unhealthy()).Execute().Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public void HealthCheck_ReturnsCorrectMessage()
        {
            var message = "message";
            new HealthCheck("test", () => HealthCheckResult.Unhealthy(message)).Execute().Check.Message.Should().Be(message);
            new HealthCheck("test", () => HealthCheckResult.Healthy(message)).Execute().Check.Message.Should().Be(message);
            new HealthCheck("test", () => { return message; }).Execute().Check.Message.Should().Be(message);
        }

        [Fact]
        public void HealthCheck_ReturnsResultWithCorrectName()
        {
            var name = "test";
            new HealthCheck(name, () => { }).Execute().Name.Should().Be(name);
            new HealthCheck(name, () => { return "string"; }).Execute().Name.Should().Be(name);
            new HealthCheck(name, () => { HealthCheckResult.Healthy(); }).Execute().Name.Should().Be(name);
        }

        [Fact]
        public void HealthCheck_SuccessIfActionDoesNotThrow()
        {
            var name = "test";
            new HealthCheck(name, () => { }).Execute().Check.IsHealthy.Should().BeTrue();
            new HealthCheck(name, () => { return "string"; }).Execute().Check.IsHealthy.Should().BeTrue();
            new HealthCheck(name, () => { HealthCheckResult.Healthy(); }).Execute().Check.IsHealthy.Should().BeTrue();
        }

        private static void ThrowException()
        {
            throw new InvalidOperationException();
        }

        private static void ThrowExceptionWithBracketsInMessage()
        {
            throw new InvalidOperationException("an {example message}");
        }
    }
}