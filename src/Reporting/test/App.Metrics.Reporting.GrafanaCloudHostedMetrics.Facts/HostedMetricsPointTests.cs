// <copyright file="HostedMetricsPointTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics.Facts
{
    public class HostedMetricsPointTests
    {
        private static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(10);

        [Fact]
        public void At_least_one_field_is_required()
        {
            // Arrange
            var fields = new Dictionary<string, object>();
            Action action = () =>
            {
                // Act
                var point = new HostedMetricsPoint(null, "measurement", fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task Can_format_payload_correctly()
        {
            // Arrange
            await using var ms = new MemoryStream();
            await using var jsonWriter = new Utf8JsonWriter(ms);
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new HostedMetricsPoint(null, "measurement", fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval, timestamp);

            // Act
            point.Write(jsonWriter);
            await  jsonWriter.FlushAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());
                
            // Assert
            result.Should().Be("{\"name\":\"measurement.key\",\"metric\":\"measurement.key\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}");
        }

        [Fact]
        public async Task Can_format_payload_with_multiple_fields_correctly()
        {
            // Arrange
            await using var ms = new MemoryStream();
            await using var jsonWriter = new Utf8JsonWriter(ms);
            var fields = new Dictionary<string, object>
                         {
                             { "field1key", "field1value" },
                             { "field2key", 2 },
                             { "field3key", false }
                         };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new HostedMetricsPoint(null, "measurement", fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval, timestamp);

            // Act
            jsonWriter.WriteStartArray();
            point.Write(jsonWriter);
            jsonWriter.WriteEndArray();
            await  jsonWriter.FlushAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should().Be("[{\"name\":\"measurement.field1key\",\"metric\":\"measurement.field1key\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"measurement.field2key\",\"metric\":\"measurement.field2key\",\"value\":2,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]},{\"name\":\"measurement.field3key\",\"metric\":\"measurement.field3key\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}]");
        }

        [Fact]
        public async Task Can_format_payload_with_tags_correctly()
        {
            // Arrange
            await using var ms = new MemoryStream();
            await using var jsonWriter = new Utf8JsonWriter(ms);
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new HostedMetricsPoint(null, "measurement", fields, tags, new DefaultHostedMetricsPointTextWriter(), FlushInterval, timestamp);

            // Act
            point.Write(jsonWriter);
            await  jsonWriter.FlushAsync();
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should()
                  .Be("{\"name\":\"measurement.tagkey.tagvalue.key\",\"metric\":\"measurement.tagkey.tagvalue.key\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}", "Hosted Metrics request at the moment allow tags array but its not yet used.");
        }

        [Fact]
        public async Task Should_add_context_when_provided()
        {
            // Arrange
            await using var ms = new MemoryStream();
            await using var jsonWriter = new Utf8JsonWriter(ms);
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new HostedMetricsPoint("context", "measurement", fields, tags, new DefaultHostedMetricsPointTextWriter(), FlushInterval, timestamp);

            // Act
            point.Write(jsonWriter);
            await  jsonWriter.FlushAsync();
            
            var result = Encoding.UTF8.GetString(ms.ToArray());

            // Assert
            result.Should().Be("{\"name\":\"context.measurement.tagkey.tagvalue.key\",\"metric\":\"context.measurement.tagkey.tagvalue.key\",\"value\":0,\"interval\":10,\"mtype\":\"gauge\",\"unit\":\"\",\"time\":1483232461,\"tags\":[]}", "Hosted Metrics request at the moment allow tags array but its not yet used.");
        }

        [Fact]
        public void Field_key_cannot_be_empty()
        {
            // Arrange
            var fields = new Dictionary<string, object> { { string.Empty, "value" } };
            Action action = () =>
            {
                // Act
                var point = new HostedMetricsPoint(null, "measurement", fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Measurement_is_required()
        {
            // Arrange
            var fields = new Dictionary<string, object> { { "key", "value" } };
            Action action = () =>
            {
                // Act
                var point = new HostedMetricsPoint(null, string.Empty, fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified, false)]
        [InlineData(DateTimeKind.Local, false)]
        [InlineData(DateTimeKind.Utc, true)]
        public void Time_stamp_should_be_utc(DateTimeKind dateTimeKind, bool expected)
        {
            // Arrange
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, dateTimeKind);

            Action action = () =>
            {
                // Act
                var point = new HostedMetricsPoint(null, "measurement", fields, MetricTags.Empty, new DefaultHostedMetricsPointTextWriter(), FlushInterval, timestamp);
            };

            // Assert
            if (!expected)
            {
                action.Should().Throw<ArgumentException>();
            }
            else
            {
                action.Should().NotThrow();
            }
        }
    }
}