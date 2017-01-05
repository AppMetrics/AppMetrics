using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class InfluxDBSettingsTests
    {
        [Fact]
        public void can_generate_influx_write_endpoint()
        {
            var settings = new InfluxDBSettings
            {
                Database = "testdb",
                RetensionPolicy = "defaultrp",
                Consistenency = "consistency"
            };

            settings.Endpoint.Should().Be("write?db=testdb&rp=defaultrp&consistency=consistency");
        }
    }
}