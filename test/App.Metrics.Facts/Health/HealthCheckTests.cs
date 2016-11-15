using System;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Health
{
    public class HealthCheckTests
    {
        [Fact]
        public async Task failed_and_does_not_throw_unhandled_exception_if_action_throws_exception_with_brackets_in_message()
        {
            var name = "test";

            var check1 = new HealthCheck(name, () => ThrowExceptionWithBracketsInMessage());
            var result1 = await check1.ExecuteAsync();
            result1.Check.IsHealthy.Should().BeFalse();

            var check2 = new HealthCheck(name, () =>
            {
                ThrowExceptionWithBracketsInMessage();
                return Task.FromResult("string");
            });
            var result2 = await check2.ExecuteAsync();
            result2.Check.IsHealthy.Should().BeFalse();

            var check3 = new HealthCheck(name, () =>
            {
                ThrowExceptionWithBracketsInMessage();
                return AppMetricsTaskCache.CompletedHealthyTask;
            });
            var result3 = await check3.ExecuteAsync();
            result3.Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public async Task failed_if_action_throws()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => ThrowException());
            var result1 = await check1.ExecuteAsync();
            result1.Check.IsHealthy.Should().BeFalse();

            var check2 = new HealthCheck(name, () =>
            {
                ThrowException();
                return Task.FromResult("string");
            });
            var result2 = await check2.ExecuteAsync();
            result2.Check.IsHealthy.Should().BeFalse();

            var check3 = new HealthCheck(name, () =>
            {
                ThrowException();
                return AppMetricsTaskCache.CompletedHealthyTask;
            });
            var result3 = await check3.ExecuteAsync();
            result3.Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public async Task failed_if_result_is_unhealthy()
        {
            var check = new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Unhealthy()));
            var result = await check.ExecuteAsync();
            result.Check.IsHealthy.Should().BeFalse();
        }

        [Fact]
        public async Task returns_correct_message()
        {
            var message = "message";
            var check1 = new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Unhealthy(message)));
            var result1 = await check1.ExecuteAsync();
            result1.Check.Message.Should().Be(message);

            var check2 = new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Healthy(message)));
            var result2 = await check2.ExecuteAsync();
            result2.Check.Message.Should().Be(message);

            var check3 = new HealthCheck("test", () => Task.FromResult(message));
            var result3 = await check3.ExecuteAsync();
            result3.Check.Message.Should().Be(message);
        }

        [Fact]
        public async Task returns_result_with_correct_name()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy()));
            var result1 = await check1.ExecuteAsync();
            result1.Name.Should().Be(name);

            var check2 = new HealthCheck(name, () => Task.FromResult("string"));
            var result2 = await check2.ExecuteAsync();
            result2.Name.Should().Be(name);

            var check3 = new HealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy()));
            var result3 = await check3.ExecuteAsync();
            result3.Name.Should().Be(name);
        }

        [Fact]
        public async Task success_if_action_does_not_throw()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy()));
            var result1 = await check1.ExecuteAsync();
            result1.Check.IsHealthy.Should().BeTrue();

            var check2 = new HealthCheck(name, () => Task.FromResult("string"));
            var result2 = await check2.ExecuteAsync();
            result2.Check.IsHealthy.Should().BeTrue();

            var check3 = new HealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy()));
            var result3 = await check3.ExecuteAsync();
            result3.Check.IsHealthy.Should().BeTrue();
        }

        private static Task<HealthCheckResult> ThrowException()
        {
            throw new InvalidOperationException();
        }

        private static Task<HealthCheckResult> ThrowExceptionWithBracketsInMessage()
        {
            throw new InvalidOperationException("an {example message}");            
        }
    }
}