// <copyright file="LineProtocolSyntaxTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
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
        public void Can_escape_name(string nameOrKey, string expected)
        {
            LineProtocolSyntax.EscapeName(nameOrKey).Should().Be(expected);
        }

        [Fact]
        public void Can_format_timespan()
        {
            var value = TimeSpan.FromMinutes(1);

            LineProtocolSyntax.FormatValue(value).Should().Be("60000");
        }

        [Fact]
        public void Can_format_timestamp()
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
        public void Can_format_value(object value, string expected)
        {
            LineProtocolSyntax.FormatValue(value).Should().Be(expected);
        }
    }
}