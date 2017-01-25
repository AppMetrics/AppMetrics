// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using App.Metrics.Tagging;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class LineProtocolPointTests
    {
        [Fact]
        public void at_least_one_field_is_required()
        {
            var fields = new Dictionary<string, object>();
            Action action = () =>
            {
                var point = new LineProtocolPoint("measurement", fields, MetricTags.None);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void can_format_payload_correctly()
        {
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPoint("measurement", fields, MetricTags.None, timestamp);

            point.Format(textWriter);

            textWriter.ToString().Should().Be("measurement key=\"value\" 1483232461000000000");
        }

        [Fact]
        public void can_format_payload_correctly_without_providing_timestamp()
        {
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var point = new LineProtocolPoint("measurement", fields, MetricTags.None);

            point.Format(textWriter);

            textWriter.ToString().Should().Be("measurement key=\"value\"");
        }

        [Fact]
        public void can_format_payload_with_multiple_fields_correctly()
        {
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object>
                         {
                             { "field1key", "field1value" },
                             { "field2key", 2 },
                             { "field3key", false }
                         };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPoint("measurement", fields, MetricTags.None, timestamp);

            point.Format(textWriter);

            textWriter.ToString().Should()
                      .Be("measurement field1key=\"field1value\",field2key=2i,field3key=f 1483232461000000000");
        }

        [Fact]
        public void can_format_payload_with_tags_correctly()
        {
            var textWriter = new StringWriter();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags().With("tagkey", "tagvalue");
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var point = new LineProtocolPoint("measurement", fields, tags, timestamp);

            point.Format(textWriter);

            textWriter.ToString().Should().Be("measurement,tagkey=tagvalue key=\"value\" 1483232461000000000");
        }

        [Fact]
        public void field_key_cannot_be_empty()
        {
            var fields = new Dictionary<string, object> { { string.Empty, "value" } };
            Action action = () =>
            {
                var point = new LineProtocolPoint("measurement", fields, MetricTags.None);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void measurement_is_required()
        {
            var fields = new Dictionary<string, object> { { "key", "value" } };
            Action action = () =>
            {
                var point = new LineProtocolPoint(string.Empty, fields, MetricTags.None);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void tag_key_cannot_be_empty()
        {
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var tags = new MetricTags().With(string.Empty, "value");

            Action action = () =>
            {
                var point = new LineProtocolPoint("measurement", fields, tags);
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Theory]
        [InlineData(DateTimeKind.Unspecified, false)]
        [InlineData(DateTimeKind.Local, false)]
        [InlineData(DateTimeKind.Utc, true)]
        public void time_stamp_should_be_utc(DateTimeKind dateTimeKind, bool expected)
        {
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var timestamp = new DateTime(2017, 1, 1, 1, 1, 1, dateTimeKind);

            Action action = () =>
            {
                var point = new LineProtocolPoint("measurement", fields, MetricTags.None, timestamp);
            };

            if (!expected)
            {
                action.ShouldThrow<ArgumentException>();
            }
            else
            {
                action.ShouldNotThrow();
            }
        }
    }
}