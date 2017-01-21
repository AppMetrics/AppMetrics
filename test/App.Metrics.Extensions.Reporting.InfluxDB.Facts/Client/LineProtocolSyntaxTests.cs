// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class LineProtocolSyntaxTests
    {
        [Theory]
        [InlineData("test=", "test\\=")]
        [InlineData("test ", "test\\ ")]
        [InlineData("test,", "test\\,")]
        [InlineData("te=st", "te\\=st")]
        [InlineData("te st", "te\\ st")]
        [InlineData("te,st", "te\\,st")]
        public void can_escape_name(string nameOrKey, string expected) { LineProtocolSyntax.EscapeName(nameOrKey).Should().Be(expected); }

        [Fact]
        public void can_format_timespan()
        {
            var value = TimeSpan.FromMinutes(1);

            LineProtocolSyntax.FormatValue(value).Should().Be("60000");
        }

        [Fact]
        public void can_format_timestamp()
        {
            var dateTime = new DateTime(2017, 01, 01, 1, 1, 1, DateTimeKind.Utc);
            LineProtocolSyntax.FormatTimestamp(dateTime).Should().Be("1483232461000000000");
        }

        [Theory]
        [InlineData(1, "1i")]
        [InlineData((sbyte)1, "1i")]
        [InlineData((byte)1, "1i")]
        [InlineData((short)1, "1i")]
        [InlineData((ushort)1, "1i")]
        [InlineData((uint)1, "1i")]
        [InlineData((long)1, "1i")]
        [InlineData((ulong)1, "1i")]
        [InlineData((float)1, "1")]
        [InlineData((double)1, "1")]
        [InlineData(true, "t")]
        [InlineData(false, "f")]
        public void can_format_value(object value, string expected) { LineProtocolSyntax.FormatValue(value).Should().Be(expected); }
    }
}