// <copyright file="AsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Ascii.Facts.Fixtures;
using App.Metrics.Health.Formatting;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class AsciiFormatterTests
    {
        private readonly HealthFixture _fixture;

        public AsciiFormatterTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new HealthFixture();
        }

        [Fact]
        public async Task Can_apply_ascii_health_formatting()
        {
            // Arrange
            _fixture.HealthCheckRegistry.Register("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()));
            var formatter = new HealthStatusPayloadFormatter();
            var payloadBuilder = new AsciiHealthStatusPayloadBuilder();

            // Act
            var healthStatus = await _fixture.Health.ReadStatusAsync();
            formatter.Build(healthStatus, payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be(
                "# OVERALL STATUS: Healthy\n--------------------------------------------------------------\n# CHECK: test\n\n           MESSAGE = OK\n            STATUS = Healthy\n--------------------------------------------------------------\n");
        }
    }
}