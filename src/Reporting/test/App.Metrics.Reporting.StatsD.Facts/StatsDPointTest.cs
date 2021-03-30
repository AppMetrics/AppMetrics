using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Formatting.StatsD;
using App.Metrics.Formatting.StatsD.Internal;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.StatsD.Facts
{
    public class StatsDPointTest
    {
        private readonly MetricsStatsDOptions _options = 
            new MetricsStatsDOptions
            {
                DefaultSampleRate = 1.0,
                MetricNameFormatter = new DefaultDogStatsDMetricStringSerializer()
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
            var point = new StatsDPoint("name.key", "value", "c", 0.1, null, new DefaultDogStatsDMetricStringSerializer(), timestamp);

            // Act
            var result = point.Write(_options);

            // Assert
            result.Should().Be("name.key:0|c|@0.1");
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
                    MetricNameFormatter = new DefaultDogStatsDMetricStringSerializer()
                });

            // Act
            statsDWriter.Write(null, "measurement", fields.Keys, fields.Values, MetricTags.Empty, timestamp);
            await statsDWriter.DisposeAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should().Be(
                "measurement.field1key:0|g\n" +
                "measurement.field2key:2|g\n" +
                "measurement.field3key:0|g");
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
                    MetricNameFormatter = new DefaultDogStatsDMetricStringSerializer()
                });

            // Act
            statsDWriter.Write(null, "measurement", fields.Keys, fields.Values, tags, timestamp);
            await statsDWriter.DisposeAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should().Be(
                "measurement.key:0|g|#tagkey:tagvalue",
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
                    MetricNameFormatter = new DefaultDogStatsDMetricStringSerializer()
                });

            // Act
            statsDWriter.Write("context", "measurement", fields.Keys, fields.Values, tags, timestamp);
            await statsDWriter.DisposeAsync();

            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should().Be(
                "context.measurement.key:0|g|#tagkey:tagvalue",
                "Hosted Metrics request at the moment allow tags array but its not yet used.");
        }

        MetricsDataValueSource CreateSource(DateTime timestamp, int gaugeCount)
        {
            GaugeValueSource CreateGauge(int id)
            {
                var gauge1 = new ValueGauge();
                gauge1.SetValue(id);
                return new GaugeValueSource($"measurement.{id}", gauge1, Unit.Events, MetricTags.Empty);
            }


            return new MetricsDataValueSource(timestamp, new[]
            {
                new MetricsContextValueSource("",
                    Enumerable.Range(0, gaugeCount).Select(CreateGauge).ToArray()
                    , new CounterValueSource[0], new MeterValueSource[0], new HistogramValueSource[0],
                    new BucketHistogramValueSource[0], new TimerValueSource[0], new BucketTimerValueSource[0],
                    new ApdexValueSource[0])
            });
        }

        [Fact]
        public async Task Can_format_CHUNKED_StatsD_payload_with_multiple_fields_correctly()
        {
            // Arrange
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var metricsData = CreateSource(timestamp, 4);

            var options = new MetricsStatsDOptions
            {
                MetricNameFormatter = new DefaultStatsDMetricStringSerializer()
            };
            var formatter = new MetricsStatsDStringOutputFormatter(options);

            // Act
            var chunks = await formatter.WriteAsync(metricsData, 30, CancellationToken.None);

            // Assert
            chunks.Count.Should().Be(4);
            chunks.All(x => !x.EndsWith("\n")).Should()
                .BeTrue("All metrics in this sample should be transmitted in their own packet.");
        }

        [Fact]
        public async Task Can_format_CHUNKED_DogStatsD_payload_with_multiple_fields_correctly()
        {
            // Arrange
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var metricsData = CreateSource(timestamp, 4);

            var options = new MetricsStatsDOptions
            {
                MetricNameFormatter = new DefaultDogStatsDMetricStringSerializer()
            };
            var formatter = new MetricsStatsDStringOutputFormatter(options);

            // Act
            var chunks = await formatter.WriteAsync(metricsData, 30, CancellationToken.None);

            // Assert
            chunks.Count.Should().Be(4);
            chunks.All(x => !x.EndsWith("\n")).Should()
                .BeTrue("All metrics in this sample should be transmitted in their own packet.");
        }
    }
}