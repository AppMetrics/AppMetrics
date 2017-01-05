using System.Collections.Generic;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public interface ILineProtocolPayloadBuilder
    {
        LineProtocolPayloadBuilder Init();

        LineProtocolPayloadBuilder Pack(string name, object value, MetricTags tags);

        LineProtocolPayloadBuilder Pack(string name, IEnumerable<string> columns, 
            IEnumerable<object> values, MetricTags tags);

        string PayloadFormatted();

        LineProtocolPayload Payload();

        void Clear();
    }
}