// <copyright file="CustomAsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Ascii.Facts.Fixtures;
using App.Metrics.Health.Formatters.Ascii.Facts.TestHelpers;
using App.Metrics.Health.Formatting;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class CustomAsciiFormatterTests
    {
        private readonly HealthFixture _fixture;

        public CustomAsciiFormatterTests()
        {
            _fixture = new HealthFixture();
        }

        [Fact]
        public async Task Can_apply_custom_ascii_health_formatting()
        {
            // Arrange
            _fixture.HealthCheckRegistry.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var formatter = new HealthStatusPayloadFormatter();
            var payloadBuilder = new CustomAsciiHealthStatusPayloadBuilder();

            // Act
            var healthStatus = await _fixture.Health.ReadStatusAsync();
            formatter.Build(healthStatus, payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("Overall: Healthy\ntest OK Healthy\n");
        }
    }
}