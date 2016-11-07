using System.Collections.Generic;

namespace App.Metrics
{
    public class Metric
    {
        private Dictionary<string, string> _tags = MetricTags.None;

        public string Name { get; set; }

        public Dictionary<string, string> Tags
        {
            get { return _tags; }
            set { _tags = value ?? new Dictionary<string, string>(); }
        }

        public string Unit { get; set; }
    }
}