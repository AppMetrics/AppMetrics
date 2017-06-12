// <copyright file="CustomAsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Core.Formatting;
using App.Metrics.Counter;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Facts.Formatting.TestHelpers;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Health;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Formatting
{
    public class CustomAsciiFormatterTests
    {
        private readonly MetricsFixture _fixture;

        public CustomAsciiFormatterTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public async Task Can_apply_custom_ascii_health_formatting()
        {
            // Arrange
            _fixture.HealthCheckFactory.Register("test", () => Task.FromResult(HealthCheckResult.Healthy()));
            var formatter = new HealthStatusPayloadFormatter();
            var payloadBuilder = new CustomAsciiHealthStatusPayloadBuilder();

            // Act
            var healthStatus = await _fixture.Metrics.Health.ReadStatusAsync();
            formatter.Build(healthStatus, payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("Overall: Healthy\ntest OK Healthy\n");
        }

        [Fact]
        public void Can_apply_custom_ascii_metric_formatting()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var formatter = new MetricDataValueSourceFormatter();
            var payloadBuilder = new CustomAsciiMetricPayloadBuilder();

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            formatter.Build(_fixture.Metrics.Snapshot.Get(), payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be("[test] counter1 counter none value = 1\n");
        }
    }
}