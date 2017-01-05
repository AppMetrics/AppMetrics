using System;
using System.Collections.Generic;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class LineProtocolClientTests
    {
        private readonly LineProtocolPayload _payload;

        public LineProtocolClientTests()
        {
            _payload = new LineProtocolPayload();
            var fieldsOne = new Dictionary<string, object> { { "key", "value" } };
            var timestampOne = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var pointOne = new LineProtocolPoint("measurement", fieldsOne, MetricTags.None, timestampOne);
            _payload.Add(pointOne);
        }

        [Fact]
        public void should_back_off_when_fails()
        {
            //TODO: AH - implement test
        }

        [Fact]
        public void influxdb_settings_are_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(new LoggerFactory(), null, new HttpPolicy());
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void http_policy_is_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(new LoggerFactory(), new InfluxDBSettings("influx", new Uri("http://localhost")), null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void databse_is_required()
        {
            Action action = () =>
            {
                var client = new DefaultLineProtocolClient(new LoggerFactory(), new InfluxDBSettings("influx", new Uri("http://localhost")), new HttpPolicy());
            };

            action.ShouldThrow<ArgumentException>();
        }
    }
}