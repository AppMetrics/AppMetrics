// <copyright file="AsciiFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.Formatters.Ascii.Facts.Fixtures;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Ascii.Facts
{
    public class AsciiFormatterTests
    {
        private readonly MetricsFixture _fixture;

        public AsciiFormatterTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public void Can_apply_ascii_metric_formatting()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var formatter = new MetricDataValueSourceFormatter();
            var payloadBuilder = new AsciiMetricPayloadBuilder();

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            formatter.Build(_fixture.Metrics.Snapshot.Get(), payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be(
                "# MEASUREMENT: [test] counter1\n# TAGS:\n             mtype = counter\n              unit = none\n# FIELDS:\n             value = 1\n--------------------------------------------------------------\n");
        }

        [Fact]
        public void Can_apply_ascii_metric_formatting_with_custom_name_formatter()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var formatter = new MetricDataValueSourceFormatter();
            var payloadBuilder = new AsciiMetricPayloadBuilder((context, name) => $"{context}---{name}");

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            formatter.Build(_fixture.Metrics.Snapshot.Get(), payloadBuilder);

            // Assert
            payloadBuilder.PayloadFormatted().Should().Be(
                "# MEASUREMENT: test---counter1\n# TAGS:\n             mtype = counter\n              unit = none\n# FIELDS:\n             value = 1\n--------------------------------------------------------------\n");
        }
    }
}