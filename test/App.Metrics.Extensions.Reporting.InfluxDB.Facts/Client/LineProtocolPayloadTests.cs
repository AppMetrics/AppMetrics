using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Client
{
    public class LineProtocolPayloadTests
    {
        [Fact]
        public void can_format_payload()
        {
            var textWriter = new StringWriter();
            var payload = new LineProtocolPayload();
            var fieldsOne = new Dictionary<string, object> { { "key", "value" } };
            var timestampOne = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var pointOne = new LineProtocolPoint("measurement", fieldsOne, MetricTags.None, timestampOne);

            var fieldsTwo = new Dictionary<string, object>
            {
                { "field1key", "field1value" },
                { "field2key", 2 },
                { "field3key", false }

            };
            var timestampTwo = new DateTime(2017, 1, 2, 1, 1, 1, DateTimeKind.Utc);
            var pointTwo = new LineProtocolPoint("measurement", fieldsTwo, MetricTags.None, timestampTwo);

            payload.Add(pointOne);
            payload.Add(pointTwo);

            payload.Format(textWriter);

            textWriter.ToString().Should()
                .Be("measurement key=\"value\" 1483232461000000000\nmeasurement field1key=\"field1value\",field2key=2i,field3key=f 1483318861000000000\n");
        }
    }
}