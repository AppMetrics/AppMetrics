using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestPayloadBuilder : IMetricPayloadBuilder<TestMetricPayload>
    {
        private TestMetricPayload _payload;

        public void Clear() { _payload = null; }

        public void Init()
        {
            _payload = new TestMetricPayload();
        }

        public void Pack(string name, object value, MetricTags tags)
        {
            _payload.Add(new TestMetricPoint(name, new Dictionary<string, object> { { "value", value } }, tags));
        }

        public void Pack(
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data })
                                .ToDictionary(pair => pair.column, pair => pair.data);

            _payload.Add(new TestMetricPoint(name, fields, tags));
        }

        public TestMetricPayload Payload() { return _payload; }

        public string PayloadFormatted()
        {
            var result = new StringBuilder();
            _payload.Format(result);
            return result.ToString();
        }
    }
}