// <copyright file="HealthCheckBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Builders
{
    public class HealthCheckBuilderTests
    {
        private static readonly ValueTask<HealthCheckResult> HealthyResult = new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());

        [Fact]
        public void Can_register_health_check_with_cancellation_token()
        {
            // Arrange
            const string check = "check with token";
            var builder = new HealthBuilder();

            // Act
            var health = builder.HealthChecks.AddCheck(check, token => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded())).Build();

            // Assert
            health.Checks.Count().Should().Be(1);
            health.Checks.First(c => c.Name == check).Should().NotBeNull();
        }

        [Fact]
        public void Can_register_health_check_without_cancellation_token()
        {
            // Arrange
            const string check = "check";
            var builder = new HealthBuilder();

            // Act
            var health = builder.HealthChecks.AddCheck(check, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded())).Build();

            // Assert
            health.Checks.Count().Should().Be(1);
            health.Checks.First(c => c.Name == check).Should().NotBeNull();
        }

        [Fact]
        public void Can_register_instance_health_checks()
        {
            // Arrange
            var builder = new HealthBuilder();
            var check = new DatabaseHealthCheck(new Database());

            // Act
            var health = builder.HealthChecks.AddCheck(check).Build();

            // Assert
            health.Checks.Count().Should().Be(1);
            health.Checks.Single().Name.Should().Be("DatabaseCheck");
        }

        [Fact]
        public void Can_register_type_health_checks()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.HealthChecks.AddCheck<SampleHealthCheck>().Build();

            // Assert
            health.Checks.Count().Should().Be(1);
            health.Checks.Single().Name.Should().Be("SampleHealthCheck");
        }

        [Fact]
        public void Can_register_mulitple_health_checks()
        {
            // Arrange
            var builder = new HealthBuilder();
            var checks = new[]
                         {
                             new HealthCheck("first", () => HealthyResult),
                             new HealthCheck("second", () => HealthyResult)
                         };

            // Act
            var health = builder.HealthChecks.AddChecks(checks).Build();

            // Assert
            health.Checks.Count().Should().Be(2);
        }

        [Fact]
        public void Cannot_set_null_health_check()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new HealthBuilder().HealthChecks.AddCheck(null, () => HealthyResult);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throws_on_duplicate_registration()
        {
            // Arrange
            var builder = new HealthBuilder().HealthChecks.AddCheck("test", () => HealthyResult);

            Action action = () =>
            {
                // Act
                builder.HealthChecks.AddCheck("test", () => HealthyResult);
                var unused = builder.Build();
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }
}