// <copyright file="InfluxDBSettingsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Reporting.InfluxDB.Facts
{
    // ReSharper disable InconsistentNaming
    public class InfluxDbSettingsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void Can_generate_influx_write_endpoint()
        {
            var settings = new InfluxDbOptions
                           {
                               Database = "testdb",
                               BaseUri = new Uri("http://localhost"),
                               RetentionPolicy = "defaultrp",
                               Consistenency = "consistency"
                           };

            settings.Endpoint.Should().Be("write?db=testdb&rp=defaultrp&consistency=consistency");
        }
    }
}