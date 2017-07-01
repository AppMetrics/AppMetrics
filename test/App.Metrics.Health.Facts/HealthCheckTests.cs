// <copyright file="HealthCheckTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts
{
    public class HealthCheckTests
    {
        [Fact]
        public async Task Failed_and_does_not_throw_unhandled_exception_if_action_throws_exception_with_brackets_in_message()
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
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("string"));
                });
            var result2 = await check2.ExecuteAsync(CancellationToken.None);
            result2.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check3 = new HealthCheck(
                name,
                () =>
                {
                    ThrowExceptionWithBracketsInMessage();
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
                });
            var result3 = await check3.ExecuteAsync(CancellationToken.None);
            result3.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task Failed_if_action_throws()
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
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("string"));
                });
            var result2 = await check2.ExecuteAsync();
            result2.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);

            var check3 = new HealthCheck(
                name,
                () =>
                {
                    ThrowException();
                    return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
                });
            var result3 = await check3.ExecuteAsync();
            result3.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task Failed_if_result_is_unhealthy()
        {
            var check = new HealthCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy()));
            var result = await check.ExecuteAsync();
            result.Check.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task Returns_correct_message()
        {
            var message = "message";
            var check1 = new HealthCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(message)));
            var result1 = await check1.ExecuteAsync();
            result1.Check.Message.Should().Be(message);

            var check2 = new HealthCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message)));
            var result2 = await check2.ExecuteAsync();
            result2.Check.Message.Should().Be(message);

            var check3 = new HealthCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy(message)));
            var result3 = await check3.ExecuteAsync();
            result3.Check.Message.Should().Be(message);

            var check4 = new HealthCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(message)));
            var result4 = await check4.ExecuteAsync();
            result4.Check.Message.Should().Be(message);
        }

        [Fact]
        public async Task Returns_result_with_correct_name()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var result1 = await check1.ExecuteAsync();
            result1.Name.Should().Be(name);

            var check2 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("string")));
            var result2 = await check2.ExecuteAsync();
            result2.Name.Should().Be(name);

            var check3 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var result3 = await check3.ExecuteAsync();
            result3.Name.Should().Be(name);
        }

        [Fact]
        public async Task Success_if_action_does_not_throw()
        {
            var name = "test";
            var check1 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var result1 = await check1.ExecuteAsync();
            result1.Check.Status.Should().Be(HealthCheckStatus.Healthy);

            var check2 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("string")));
            var result2 = await check2.ExecuteAsync();
            result2.Check.Status.Should().Be(HealthCheckStatus.Healthy);

            var check3 = new HealthCheck(name, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var result3 = await check3.ExecuteAsync();
            result3.Check.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        private static ValueTask<HealthCheckResult> ThrowException() { throw new InvalidOperationException(); }

        private static ValueTask<HealthCheckResult> ThrowExceptionWithBracketsInMessage() { throw new InvalidOperationException("an {example message}"); }
    }
}