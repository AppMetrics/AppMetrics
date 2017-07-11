// <copyright file="CustomAsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Formatters.Ascii.Facts.Fixtures;
using App.Metrics.Formatters.Ascii.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Ascii.Facts
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