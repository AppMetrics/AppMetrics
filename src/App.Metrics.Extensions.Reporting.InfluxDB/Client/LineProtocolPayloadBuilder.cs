// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public class LineProtocolPayloadBuilder : ILineProtocolPayloadBuilder
    {
        private LineProtocolPayload _payload;

        public void Clear() { _payload = null; }

        public LineProtocolPayloadBuilder Init()
        {
            _payload = new LineProtocolPayload();
            return this;
        }

        public LineProtocolPayloadBuilder Pack(string name, object value, MetricTags tags)
        {
            _payload.Add(new LineProtocolPoint(name, new Dictionary<string, object> { { "value", value } }, tags));

            return this;
        }

        public LineProtocolPayloadBuilder Pack(
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data })
                                .ToDictionary(pair => pair.column, pair => pair.data);

            _payload.Add(new LineProtocolPoint(name, fields, tags));

            return this;
        }

        public LineProtocolPayload Payload() { return _payload; }

        public string PayloadFormatted()
        {
            var result = new StringWriter();
            _payload.Format(result);
            return result.ToString();
        }
    }
}