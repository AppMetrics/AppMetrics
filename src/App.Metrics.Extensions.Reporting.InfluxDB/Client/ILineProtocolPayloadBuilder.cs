// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public interface ILineProtocolPayloadBuilder
    {
        void Clear();

        LineProtocolPayloadBuilder Init();

        LineProtocolPayloadBuilder Pack(string name, object value, MetricTags tags);

        LineProtocolPayloadBuilder Pack(
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags);

        LineProtocolPayload Payload();

        string PayloadFormatted();
    }
}