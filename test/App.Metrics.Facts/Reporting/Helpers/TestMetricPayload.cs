using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestMetricPayload
    {
        private readonly List<TestMetricPoint> _points = new List<TestMetricPoint>();

        public void Add(TestMetricPoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public void Format(StringBuilder sb)
        {
            var points = Enumerable.ToList<TestMetricPoint>(_points);

            foreach (var point in points)
            {
                point.Format(sb);
                sb.AppendLine();
            }
        }
    }
}