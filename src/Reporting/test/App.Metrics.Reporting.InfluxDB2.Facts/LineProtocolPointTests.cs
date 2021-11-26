// <copyright file="LineProtocolPointTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Formatters.InfluxDB.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
{
    public class LineProtocolPointTests
    {
        [Fact]
        public void At_least_one_field_is_required_for_multiple_values()
        {
            var fields = new List<string>();
            var values = new List<object>();
            Action action = () =>
            {
                var unused = new LineProtocolPointMultipleValues("measurement", fields, values, MetricTags.Empty);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void At_least_one_field_is_required_for_single_value()
        {
            var fieldName = string.Empty;
            var fieldValue = "value";
            Action action = () =>
            {
                var unused = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, MetricTags.Empty);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Can_format_payload_correctly()
        {
            var textWriter = new StringWriter();
            var fieldName = "key";
            var fieldValue = "value";
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, MetricTags.Empty, timestamp);

            point.WriteAsync(textWriter);

            textWriter.ToString().Should().Be("measurement key=\"value\" 1483232461000000000");
        }

        [Fact]
        public void Can_format_payload_correctly_without_providing_timestamp()
        {
            var textWriter = new StringWriter();
            var fieldName = "key";
            var fieldValue = "value";
            var point = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, MetricTags.Empty);

            point.WriteAsync(textWriter, false);

            textWriter.ToString().Should().Be("measurement key=\"value\"");
        }

        [Fact]
        public void Can_format_payload_with_multiple_fields_correctly()
        {
            var textWriter = new StringWriter();
            var fieldsNames = new[] { "field1key", "field2key", "field3key" };
            var fieldsValues = new object[] { "field1value", 2, false };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPointMultipleValues("measurement", fieldsNames, fieldsValues, MetricTags.Empty, timestamp);

            point.WriteAsync(textWriter);

            textWriter.ToString().Should().Be("measurement field1key=\"field1value\",field2key=2i,field3key=f 1483232461000000000");
        }

        [Fact]
        public void Can_format_payload_with_tags_correctly()
        {
            var textWriter = new StringWriter();
            var fieldName = "key";
            var fieldValue = "value";
            var tags = new MetricTags("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, tags, timestamp);

            point.WriteAsync(textWriter);

            textWriter.ToString().Should().Be("measurement,tagkey=tagvalue key=\"value\" 1483232461000000000");
        }

        [Fact]
        public void Field_key_cannot_be_empty_for_single_value()
        {
            var fieldName = string.Empty;
            var fieldValue = "value";
            Action action = () =>
            {
                var unused = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, MetricTags.Empty);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Field_key_cannot_be_empty_for_multiple_values()
        {
            var fieldsNames = new[] { string.Empty };
            var fieldsValues = new object[] { "values" };
            Action action = () =>
            {
                var unused = new LineProtocolPointMultipleValues("measurement", fieldsNames, fieldsValues, MetricTags.Empty);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Measurement_is_required()
        {
            var fieldName = "key";
            var fieldValue = "value";
            Action action = () =>
            {
                var unused = new LineProtocolPointSingleValue(string.Empty, fieldName, fieldValue, MetricTags.Empty);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified, false)]
        [InlineData(DateTimeKind.Local, false)]
        [InlineData(DateTimeKind.Utc, true)]
        public void Time_stamp_should_be_utc(DateTimeKind dateTimeKind, bool expected)
        {
            var fieldName = "key";
            var fieldValue = "value";
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, dateTimeKind);

            Action action = () =>
            {
                var unused = new LineProtocolPointSingleValue("measurement", fieldName, fieldValue, MetricTags.Empty, timestamp);
            };

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