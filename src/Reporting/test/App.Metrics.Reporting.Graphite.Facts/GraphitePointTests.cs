// <copyright file="GraphitePointTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Formatters.Graphite.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.Graphite.Facts
{
    public class GraphitePointTests
    {
        [Fact]
        public void At_least_one_field_is_required()
        {
            // Arrange
            var fields = new Dictionary<string, object>();
            Action action = () =>
            {
                // Act
                var point = new GraphitePoint(null, "measurement", fields, MetricTags.Empty, new DefaultGraphitePointTextWriter());
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Can_format_payload_correctly()
        {
            // Arrange
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new GraphitePoint(null, "measurement", fields, MetricTags.Empty, new DefaultGraphitePointTextWriter(), timestamp);

            // Act
            point.WriteAsync(textWriter);

            // Assert
            textWriter.ToString().Should().Be("measurement.key value 1483232461\n");
        }

        [Fact]
        public void Can_format_payload_with_multiple_fields_correctly()
        {
            // Arrange
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object>
                         {
                             { "field1key", "field1value" },
                             { "field2key", 2 },
                             { "field3key", false }
                         };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new GraphitePoint(null, "measurement", fields, MetricTags.Empty, new DefaultGraphitePointTextWriter(), timestamp);

            // Act
            point.WriteAsync(textWriter);

            // Assert
            textWriter.ToString().Should()
                      .Be("measurement.field1key field1value 1483232461\nmeasurement.field2key 2 1483232461\nmeasurement.field3key f 1483232461\n");
        }

        [Fact]
        public void Can_format_payload_with_tags_correctly()
        {
            // Arrange
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new GraphitePoint(null, "measurement", fields, tags, new DefaultGraphitePointTextWriter(), timestamp);

            // Act
            point.WriteAsync(textWriter);

            // Assert
            textWriter.ToString().Should().Be("measurement.tagkey.tagvalue.key value 1483232461\n");
        }

        [Fact]
        public void Should_add_context_when_provided()
        {
            // Arrange
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new GraphitePoint("context", "measurement", fields, tags, new DefaultGraphitePointTextWriter(), timestamp);

            // Act
            point.WriteAsync(textWriter);

            // Assert
            textWriter.ToString().Should().Be("context.measurement.tagkey.tagvalue.key value 1483232461\n");
        }

        [Fact]
        public void Field_key_cannot_be_empty()
        {
            // Arrange
            var fields = new Dictionary<string, object> { { string.Empty, "value" } };
            Action action = () =>
            {
                // Act
                var point = new GraphitePoint(null, "measurement", fields, MetricTags.Empty, new DefaultGraphitePointTextWriter());
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
                var point = new GraphitePoint(null, string.Empty, fields, MetricTags.Empty, new DefaultGraphitePointTextWriter());
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
                var point = new GraphitePoint(null, "measurement", fields, MetricTags.Empty, new DefaultGraphitePointTextWriter(), timestamp);
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