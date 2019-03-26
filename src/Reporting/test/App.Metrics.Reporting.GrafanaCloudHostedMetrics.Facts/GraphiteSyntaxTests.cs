// <copyright file="GraphiteSyntaxTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics.Facts
{
    public class GraphiteSyntaxTests
    {
        [Theory]
        [InlineData("test=", "test_")]
        [InlineData("test ", "test_")]
        [InlineData("test,", "test_")]
        [InlineData("te=st", "te_st")]
        [InlineData("te st", "te_st")]
        [InlineData("te,st", "te_st")]
        [InlineData("te.st", "te.st")]
        public void Can_escape_name(string nameOrKey, string expected) { GraphiteSyntax.EscapeName(nameOrKey, true).Should().Be(expected); }

        [Fact]
        public void Can_format_timespan()
        {
            var value = TimeSpan.FromMinutes(1);

            GraphiteSyntax.FormatValue(value).Should().Be("60000");
        }

        [Fact]
        public void Can_format_timestamp()
        {
            var dateTime = new DateTime(2017, 01, 01, 1, 1, 1, DateTimeKind.Utc);
            GraphiteSyntax.FormatTimestamp(dateTime).Should().Be("1483232461");
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData((sbyte)1, "1")]
        [InlineData((byte)1, "1")]
        [InlineData((short)1, "1")]
        [InlineData((ushort)1, "1")]
        [InlineData((uint)1, "1")]
        [InlineData((long)1, "1")]
        [InlineData((ulong)1, "1")]
        [InlineData((float)1, "1.00")]
        [InlineData((double)1, "1.00")]
        [InlineData(true, "t")]
        [InlineData(false, "f")]
        public void Can_format_value(object value, string expected) { GraphiteSyntax.FormatValue(value).Should().Be(expected); }
    }
}