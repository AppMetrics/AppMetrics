// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    // ReSharper disable InconsistentNaming
    public class InfluxDBSettingsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void base_address_cannot_be_null()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings("influx", null)
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void can_generate_influx_write_endpoint()
        {
            var settings = new InfluxDBSettings("testdb", new Uri("http://localhost"))
                           {
                               RetensionPolicy = "defaultrp",
                               Consistenency = "consistency"
                           };

            settings.Endpoint.Should().Be("write?db=testdb&rp=defaultrp&consistency=consistency");
        }

        [Fact]
        public void database_cannot_be_empty()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(string.Empty, new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void database_cannot_be_null()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(null, new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void database_cannot_be_whitespace()
        {
            Action action = () =>
            {
                var settings = new InfluxDBSettings(" ", new Uri("http://localhost"))
                               {
                                   RetensionPolicy = "defaultrp",
                                   Consistenency = "consistency"
                               };
            };

            action.ShouldThrow<ArgumentException>();
        }
    }
}