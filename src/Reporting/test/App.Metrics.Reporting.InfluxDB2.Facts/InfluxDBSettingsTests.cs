// <copyright file="InfluxDBSettingsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.InfluxDB2;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
{
    // ReSharper disable InconsistentNaming
    public class InfluxDb2SettingsTests
    // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void Can_generate_influx_write_endpoint()
        {
            var settings = new InfluxDb2Options
            {
                Organization = "testorg",
                Bucket = "testbucket",
                BaseUri = new Uri("http://localhost"),
                Precision = "s"
            };

            settings.Endpoint.Should().Be("api/v2/write?org=testorg&bucket=testbucket&precision=s");
        }
    }
}