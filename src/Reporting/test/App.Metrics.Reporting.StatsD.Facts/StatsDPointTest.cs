using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatting.StatsD;
using App.Metrics.Formatting.StatsD.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.StatsD.Facts
{
    public class StatsDPointTest
    {
        private readonly MetricsStatsDOptions _options = new MetricsStatsDOptions
        {
            WriteTags = true,
            WriteTimestamp = true,
            DefaultSampleRate = 1.0
        };

        [Theory]
        [InlineData(null, 1, "c", DateTimeKind.Utc, "Name must be non-null")]
        [InlineData("", 1, "c", DateTimeKind.Utc, "Name must be non-null")]
        [InlineData("  ", 1, "c", DateTimeKind.Utc, "Name must be non-null")]
        [InlineData("name", null, "c", DateTimeKind.Utc, "A value must be specified")]
        [InlineData("name", 1, null, DateTimeKind.Utc, "Metric type must be")]
        [InlineData("name", 1, "", DateTimeKind.Utc, "Metric type must be")]
        [InlineData("name", 1, " ", DateTimeKind.Utc, "Metric type must be")]
        [InlineData("name", 1, "c", DateTimeKind.Local, "Timestamps must be")]
        [InlineData("name", 1, "c", DateTimeKind.Unspecified, "Timestamps must be")]
        public void Bad_parameters_should_throws(string name, object value, string metricType, DateTimeKind dateTimeKind, string message)
        {
            // Arrange
            var tags = new Dictionary<string, string>();
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, dateTimeKind);
            Action action = () =>
            {
                // Act
                var point = new StatsDPoint(name, value, metricType, null, tags, new DefaultStatsDMetricStringSerializer(), timestamp);
            };

            // Assert
            action.Should().Throw<ArgumentException>().Where(e => e.Message.StartsWith(message));
        }

        [Fact]
        public void Can_format_payload_correctly()
        {
            // Arrange
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new StatsDPoint("name.key", "value", "c", 0.1, null, new DefaultStatsDMetricStringSerializer(), timestamp);

            // Act
            var result = point.Write(_options);

            // Assert
            result.Should().Be("name.key:0|c|@0.1|#timestamp:1483232461");
        }

        [Fact]
        public async Task Can_format_payload_with_multiple_fields_correctly()
        {
            // Arrange
            await using var ms = new MemoryStream();
            var fields = new Dictionary<string, object>
                         {
                             { "field1key", "field1value" },
                             { "field2key", 2 },
                             { "field3key", false }
                         };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);

            var statsDWriter = new MetricSnapshotStatsDStringWriter(
                ms, 
                new StatsDPointSampler(_options), 
                new MetricsStatsDOptions
                {
                    WriteTags = true,
                    WriteTimestamp = true
                });

            // Act
            statsDWriter.Write(null, "measurement", fields.Keys, fields.Values, MetricTags.Empty, timestamp);
            await statsDWriter.DisposeAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should()
                .Be("measurement.field1key:0|g|#timestamp:1483232461\n" +
                    "measurement.field2key:2|g|#timestamp:1483232461\n" +
                    "measurement.field3key:0|g|#timestamp:1483232461");
        }

        [Fact]
        public async Task Can_format_payload_with_tags_correctly()
        {
            // Arrange
            await using var ms = new MemoryStream();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);

            var statsDWriter = new MetricSnapshotStatsDStringWriter(
                ms,
                new StatsDPointSampler(_options),
                new MetricsStatsDOptions
                {
                    WriteTags = true,
                    WriteTimestamp = true
                });

            // Act
            statsDWriter.Write(null, "measurement", fields.Keys, fields.Values, tags, timestamp);
            await statsDWriter.DisposeAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should()
                .Be(
                    "measurement.key:0|g|#tagkey:tagvalue,timestamp:1483232461", 
                    "Hosted Metrics request at the moment allow tags array but its not yet used.");
        }

        [Fact]
        public async Task Should_add_context_when_provided()
        {
            // Arrange
            await using var ms = new MemoryStream();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            //var point = new DatadogPoint("context", "measurement", fields, tags, new DefaultDatadogMetricJsonWriter(), FlushInterval, timestamp);

            var statsDWriter = new MetricSnapshotStatsDStringWriter(
                ms,
                new StatsDPointSampler(_options),
                new MetricsStatsDOptions
                {
                    WriteTags = true,
                    WriteTimestamp = true
                });

            // Act
            statsDWriter.Write("context", "measurement", fields.Keys, fields.Values, tags, timestamp);
            await statsDWriter.DisposeAsync();

            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should()
                .Be(
                    "context.measurement.key:0|g|#tagkey:tagvalue,timestamp:1483232461", 
                    "Hosted Metrics request at the moment allow tags array but its not yet used.");
        }
    }
}
