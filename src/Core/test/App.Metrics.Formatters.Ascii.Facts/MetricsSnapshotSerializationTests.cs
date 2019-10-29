// <copyright file="MetricsSnapshotSerializationTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Ascii.Facts
{
    public class MetricsSnapshotSerializationTests
    {
        private readonly MetricsFixture _fixture;

        public MetricsSnapshotSerializationTests()
        {
            // DEVNOTE: Don't want Metrics to be shared between tests
            _fixture = new MetricsFixture();
        }

        [Fact]
        public async Task Can_apply_ascii_metric_formatting()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            await using var sw = new StringWriter();
            await using (var writer = new MetricSnapshotTextWriter(sw))
            {
                serializer.Serialize(writer, _fixture.Metrics.Snapshot.Get(), fields);
            }

            // Assert
            sw.ToString().Should().Be(
                "# TIMESTAMP: 0\n# MEASUREMENT: [test] counter1\n# TAGS:\n                  mtype = counter\n                   unit = none\n# FIELDS:\n                  value = 1\n--------------------------------------------------------------\n");
        }

        [Fact]
        public async Task Can_apply_ascii_metric_formatting_with_custom_name_formatter()
        {
            // Arrange
            var counter = new CounterOptions { Context = "test", Name = "counter1" };
            var serializer = new MetricSnapshotSerializer();
            var fields = new MetricFields();

            // Act
            _fixture.Metrics.Measure.Counter.Increment(counter);
            using (var sw = new StringWriter())
            {
                await using (var packer = new MetricSnapshotTextWriter(sw, metricNameFormatter: (context, name) => $"{context}---{name}"))
                {
                    serializer.Serialize(packer, _fixture.Metrics.Snapshot.Get(), fields);
                }

                // Assert
                sw.ToString().Should().Be(
                    "# TIMESTAMP: 0\n# MEASUREMENT: test---counter1\n# TAGS:\n                  mtype = counter\n                   unit = none\n# FIELDS:\n                  value = 1\n--------------------------------------------------------------\n");
            }
        }
    }
}