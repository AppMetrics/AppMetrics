using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
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
            var result1 = await check1.ExecuteAsync(CancellationToken.None);
            result1.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check2 = new HealthCheck(
                name,
                () =>
                {
                    ThrowExceptionWithBracketsInMessage();
                    return Task.FromResult("string");
                });
            var result2 = await check2.ExecuteAsync(CancellationToken.None);
            result2.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check3 = new HealthCheck(
                name,
                () =>
                {
                    ThrowExceptionWithBracketsInMessage();
                    return AppMetricsTaskCache.CompletedHealthyTask;
                });
            var result3 = await check3.ExecuteAsync(CancellationToken.None);
            result3.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task failed_if_action_throws()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => ThrowException());
            var result1 = await check1.ExecuteAsync(CancellationToken.None);
            result1.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check2 = new HealthCheck(
                name,
                () =>
                {
                    ThrowException();
                    return Task.FromResult("string");
                });
            var result2 = await check2.ExecuteAsync();
            result2.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check3 = new HealthCheck(
                name,
                () =>
                {
                    ThrowException();
                    return AppMetricsTaskCache.CompletedHealthyTask;
                });
            var result3 = await check3.ExecuteAsync();
            result3.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task failed_if_result_is_unhealthy()
        {
            var check = new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Unhealthy()));
            var result = await check.ExecuteAsync();
            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
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

            var check4 = new HealthCheck("test", () => Task.FromResult(HealthCheckResult.Degraded(message)));
            var result4 = await check4.ExecuteAsync();
            result4.Check.Message.Should().Be(message);
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
            result1.Check.Status.Should().Be(HealthCheckStatus.Healthy);

            var check2 = new HealthCheck(name, () => Task.FromResult("string"));
            var result2 = await check2.ExecuteAsync();
            result2.Check.Status.Should().Be(HealthCheckStatus.Healthy);

            var check3 = new HealthCheck(name, () => Task.FromResult(HealthCheckResult.Healthy()));
            var result3 = await check3.ExecuteAsync();
            result3.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        private static Task<HealthCheckResult> ThrowException() { throw new InvalidOperationException(); }

        private static Task<HealthCheckResult> ThrowExceptionWithBracketsInMessage() { throw new InvalidOperationException("an {example message}"); }
    }
}