using System.Collections.Generic;
using App.Metrics.Reporting;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class CustomDataKeys
    {
        public CustomDataKeys()
        {
            Histogram = new Dictionary<HistogramDataKeys, string>();
            Meter = new Dictionary<MeterValueDataKeys, string>();
            Apdex = new Dictionary<ApdexValueDataKeys, string>();
        }

        public CustomDataKeys(
            Dictionary<HistogramDataKeys, string> histogram, 
            Dictionary<MeterValueDataKeys, string> meter, 
            Dictionary<ApdexValueDataKeys, string> apdex)
        {
            Histogram = histogram;
            Meter = meter;
            Apdex = apdex;
        }

        public Dictionary<HistogramDataKeys, string> Histogram { get; }
        public Dictionary<MeterValueDataKeys, string> Meter { get; }
        public Dictionary<ApdexValueDataKeys, string> Apdex { get; }
    }
}