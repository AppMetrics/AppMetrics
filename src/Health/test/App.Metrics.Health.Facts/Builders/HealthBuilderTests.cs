// <copyright file="HealthBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Internal.NoOp;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Facts.Builders
{
    public class HealthBuilderTests
    {
        [Fact]
        public void When_no_checks_registered_use_noop_runner()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.OutputHealth.AsPlainText().Build();

            // Assert
            health.HealthCheckRunner.Should().BeOfType<NoOpHealthCheckRunner>();
        }

        [Fact]
        public void When_checks_registered_should_not_use_noop_runner()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.HealthChecks.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy())).Build();

            // Assert
            health.HealthCheckRunner.Should().NotBeOfType<NoOpHealthCheckRunner>();
        }

        [Fact]
        public void Options_should_not_be_null_when_configuration_not_used()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.Build();

            // Assert
            health.Options.Should().NotBeNull();
        }

        [Fact]
        public void Formatter_should_default_to_text_if_not_configured()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.Build();

            // Assert
            health.OutputHealthFormatters.Should().NotBeNull();
            health.OutputHealthFormatters.Single().Should().BeOfType<HealthStatusTextOutputFormatter>();
        }

        [Fact]
        public void Default_formatter_should_default_to_text_if_not_configured()
        {
            // Arrange
            var builder = new HealthBuilder();

            // Act
            var health = builder.Build();

            // Assert
            health.DefaultOutputHealthFormatter.Should().NotBeNull();
            health.DefaultOutputHealthFormatter.Should().BeOfType<HealthStatusTextOutputFormatter>();
        }
    }
}
